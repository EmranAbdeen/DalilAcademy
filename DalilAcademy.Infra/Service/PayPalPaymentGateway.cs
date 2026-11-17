using DalilAcademy.Core.Service;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static DalilAcademy.Core.DTOs.PaymentGatewayDto;

namespace DalilAcademy.Infra.Service
{
    public class PayPalPaymentGateway(HttpClient http, IConfiguration configuration) : IPaymentGateway
    {
        // 1. Get Access Token
        private async Task<string> GetAccessTokenAsync()
        {
            var clientId = configuration["PayPal:ClientId"];
            var secret = configuration["PayPal:ClientSecret"];

            Console.WriteLine($"ClientId exists: {!string.IsNullOrWhiteSpace(clientId)}");
            Console.WriteLine($"Secret exists: {!string.IsNullOrWhiteSpace(secret)}");
            Console.WriteLine($"ClientId length: {clientId?.Length}");
            Console.WriteLine($"Secret length: {secret?.Length}");

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("PayPal sandbox credentials are missing. Configure PayPal:ClientId and PayPal:ClientSecret in User Secrets.");

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

            using var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials"
                }
            );

            using var response = await http.SendAsync(request);

            response.EnsureSuccessStatusCode();
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            return document.RootElement.GetProperty("access_token").GetString()!;
        }
        public async Task<CreatePaymentOrderResult> CreateOrderAsync(decimal amountInUsd)
        {
            var accessToken = await GetAccessTokenAsync();
            var returnUrl = configuration["PayPal:ReturnUrl"];

            var orderData = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = amountInUsd.ToString(
                                "F2",
                                CultureInfo.InvariantCulture
                            )
                        }
                    }
                },

                application_context = new
                {
                    return_url = returnUrl,
                    cancel_url = returnUrl
                }
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "v2/checkout/orders");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(orderData),
                Encoding.UTF8,
                "application/json"
            );

            using var response = await http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = document.RootElement;

            var orderId = root.GetProperty("id").GetString()!;
            var approvalLink = root
                .GetProperty("links")
                .EnumerateArray()
                .FirstOrDefault(x => x.GetProperty("rel").GetString() is "approve" or "payer-action");

            if (approvalLink.ValueKind == JsonValueKind.Undefined ||
                !approvalLink.TryGetProperty("href", out var href) ||
                string.IsNullOrWhiteSpace(href.GetString()))
                throw new HttpRequestException("PayPal did not return an approval link for the order.");

            var approvalUrl = href.GetString()!;

            return new CreatePaymentOrderResult(orderId, approvalUrl);
        }


        // 3. Capture Order
        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            var accessToken = await GetAccessTokenAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v2/checkout/orders/{Uri.EscapeDataString(orderId)}/capture");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            using var response = await http.SendAsync(request);

            // Any unsuccessful HTTP response means capture failed
            if (!response.IsSuccessStatusCode)
                return false;

            using var document = JsonDocument.Parse( await response.Content.ReadAsStringAsync());
            if (!document.RootElement.TryGetProperty("status", out var statusElement))
                return false;
            var status = statusElement.GetString();

            return status == "COMPLETED";
        }
    }
}
