using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace EquipmentRentalUI.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetProtectedDataAsync(string endpoint, ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
                _httpClient.DefaultRequestHeaders.Add("X-User-Email", email);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content, ClaimsPrincipal user)
        {
            var response = await _httpClient.PostAsync(endpoint, content);
            return response;
        }

    }
}
