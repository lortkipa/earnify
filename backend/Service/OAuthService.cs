using Microsoft.Extensions.Configuration;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Service
{
    public interface IOAuthService
    {
        Task<OAuthResponseDTO?> GoogleAuth(string code);
    }

    public class OAuthService : IOAuthService
    {
        string clientId = "479552883589-iedcfq6hclq484gbr6jt6kpb2otl1l1h.apps.googleusercontent.com";
        string clientSecret = "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";
        string redirectUri = "https://localhost:7067/api/Auth/Google/Callback";

        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OAuthResponseDTO?> GoogleAuth(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var tokenRequestParams = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", redirectUri }
                };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(tokenRequestParams)
            };

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            using var jsonDoc = JsonDocument.Parse(content);
            var tokenRoot = jsonDoc.RootElement;

            var accessToken = tokenRoot.GetProperty("access_token").GetString();

            string? refreshToken = null;
            if (tokenRoot.TryGetProperty("refresh_token", out var rtElement))
            {
                refreshToken = rtElement.GetString();
            }

            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
            userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await client.SendAsync(userInfoRequest);
            if (!userInfoResponse.IsSuccessStatusCode) return null;

            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();

            using var userDoc = JsonDocument.Parse(userInfoContent);
            var userRoot = userDoc.RootElement;

            return new OAuthResponseDTO
            {
                ProviderId = userRoot.GetProperty("id").GetString() ?? "",
                RefreshToken = refreshToken,
                Email = userRoot.GetProperty("email").GetString() ?? "",
                FirstName = userRoot.GetProperty("given_name").GetString() ?? "",
                LastName = userRoot.GetProperty("family_name").GetString() ?? "",
                AvatarUrl = userRoot.GetProperty("picture").GetString() ?? ""
            };
        }
    }
}
