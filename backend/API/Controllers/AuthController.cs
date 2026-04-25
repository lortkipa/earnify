using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;
using Service.Helpers;
using System.Net.Http.Headers;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        string clientId = "479552883589-iedcfq6hclq484gbr6jt6kpb2otl1l1h.apps.googleusercontent.com";
        string clientSecret = "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";
        string redirectUri = "https://localhost:7067/api/Auth/Google/Callback";

        private readonly IUserService _userService;
        private readonly IOAuthService _oAuthService;
        private readonly IConfiguration _config;

        public AuthController(IOAuthService oAuthService, IUserService userService, IConfiguration config)
        {
            _oAuthService = oAuthService;
            _userService = userService;
            _config = config;
        }

        [HttpGet("Google")]
        public async Task<IActionResult> Google()
        {
            var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?client_id={clientId}" +
                      $"&redirect_uri={redirectUri}" +
                      $"&response_type=code" +
                      $"&scope=openid%20email%20profile" +
                      $"&access_type=offline" +
                      $"&prompt=select_account%20consent";

            return Redirect(url);
        }

        [HttpGet("Google/Callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string? code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error)) return Redirect("http://localhost:4200/home");

            var userData = await _oAuthService.GoogleAuth(code);
            if (userData == null) Redirect("http://localhost:4200/home");

            var user = await _userService.GetByGoogleIdAsync(userData.ProviderId);
            if (user == null)
            {
                user = await _userService.CreateAsync(new CreateUserDTO
                {
                    GoogleId = userData.ProviderId,
                    GoogleToken = userData.RefreshToken  ?? "",
                    Email = userData.Email,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    AvatarUrl = userData.AvatarUrl,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var token = TokenHelper.GenerateToken(user.Id, user.Email, _config);
            HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            return Redirect("http://localhost:4200/dashboard");
        }
    }
}
