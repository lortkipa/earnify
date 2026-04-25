using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("CreateDonation")]
        public async Task<JsonResult> CreateDonation([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }

            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accessToken = await GetAccessToken();

            string api = "https://api-m.sandbox.paypal.com";
            string url = api + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");
            
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { Id = paypalOrderId });
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }

        [HttpPost("CompleteDonation")]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderId"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }

            string accessToken = await GetAccessToken();

            string api = "https://api-m.sandbox.paypal.com";
            string url = api + "/v2/checkout/orders/" + orderId + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        string orderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if (orderStatus == "COMPLETED")
                        {
                            return new JsonResult("success");
                        }
                    }
                }
            }

            return new JsonResult("error");
        }

        private async Task<string> GetAccessToken()
        {
            string clientId = "AcKAtvUNG0ZctjrU51o6oOEyLna3jJjDbM12HpZNZizm67DNsKFI00L_tvmoHJw-S1_fCdX2ImK_OvB9";
            string clientSecret = "EFRpHP7okAyoKqjgABxXByA65uoRJfXjoUefIz21-BpGRGUwmGukwnUJtResQCJHKDFM-uhZ-PF22T7g";
            string api = "https://api-m.sandbox.paypal.com";
            string accessToken = "";

            string url = api + "/v1/oauth2/token";
            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMsg = new HttpRequestMessage(HttpMethod.Post, url);
                requestMsg.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMsg);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        return jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }

            return "";
        }
    }
}
