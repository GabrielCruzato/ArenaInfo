using ArenaInfo.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArenaInfo.Services
{
    public class BlizzardService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _cachedToken;
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
                    new AuthenticationHeaderValue("Basic", authHeader);

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
                _cachedToken = tokenResponse?.access_token ?? string.Empty;
                return _cachedToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetTokenAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
        }

        public async Task<PvpLeaderboard> GetPvpLeaderboardAsync(int season, string bracket = "3v3")
        {
            if (string.IsNullOrEmpty(_cachedToken))
                await GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _cachedToken);

            var url = $"https://us.api.blizzard.com/data/wow/pvp-season/{season}/pvp-leaderboard/{bracket}?namespace=dynamic-us";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PvpLeaderboard>(json);
        }

        public async Task<PvpSeasonIndex> GetPvpSeasonIndexAsync()
        {
            if (string.IsNullOrEmpty(_cachedToken))
                await GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _cachedToken);

            var url = $"https://us.api.blizzard.com/data/wow/pvp-season/index?namespace=dynamic-us&locale=en_US";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PvpSeasonIndex>(json);
        }
    }
}
