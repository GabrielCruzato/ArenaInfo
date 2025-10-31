using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ArenaInfo.Models;

namespace ArenaInfo.Services
{
    public class BlizzardService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BlizzardService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                var clientId = _configuration["BlizzardApi:ClientId"];
                var clientSecret = _configuration["BlizzardApi:ClientSecret"];

                if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                    throw new Exception("Client ID or Secret is missing in appsettings.");

                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                };
                var content = new FormUrlEncodedContent(formData);

                var tokenUrl = "https://us.battle.net/oauth/token"; 
                var response = await _httpClient.PostAsync(tokenUrl, content);

                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Body: {responseBody}");

                if (!response.IsSuccessStatusCode)
                    return $"Error {response.StatusCode}: {responseBody}";

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody);

                return tokenResponse?.access_token ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetTokenAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
        }
    }
}
