using System.Security.Cryptography;
using System.Text;

namespace SwitchBotApi
{
    public static class ApiHelper
    {
        public static async Task SendCommandAsync(string authToken, string authSecret, string deviceId, string json)
        {
            long timestamp = GetTimestamp();
            string nonce = GenerateNonce();
            string data = authToken + timestamp.ToString() + nonce;
            string signature = GenerateSignature(authSecret, data);

            using HttpClient client = new();
            var request = new HttpRequestMessage(HttpMethod.Post, $@"https://api.switch-bot.com/v1.0/devices/{deviceId}/commands")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.TryAddWithoutValidation("Authorization", authToken);
            request.Headers.TryAddWithoutValidation("sign", signature);
            request.Headers.TryAddWithoutValidation("nonce", nonce);
            request.Headers.TryAddWithoutValidation("t", timestamp.ToString());

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMessage);
            }
        }

        private static long GetTimestamp()
        {
            DateTime dt1970 = new(1970, 1, 1);
            DateTime current = DateTime.UtcNow;
            TimeSpan span = current - dt1970;
            return Convert.ToInt64(span.TotalMilliseconds);
        }

        private static string GenerateNonce()
        {
            return Guid.NewGuid().ToString();
        }

        private static string GenerateSignature(string secret, string data)
        {
            using HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(secret));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }
    }
}

