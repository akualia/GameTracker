using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using System.Net.Http.Json;
using GameTracker.Models; // Import the Game model

namespace GameTracker.Services
{
    public class RawgApiService
    {
        // api serviece for fetching game data from RAWG API
        private readonly HttpClient _httpClient;
        private string _apiKey;
        private string _baseUrl;

        public RawgApiService()
        {
            _httpClient = new HttpClient();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            try
            {
                var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("RawgApi", out var rawgApi))
                        {
                            if (rawgApi.TryGetProperty("ApiKey", out var apiKeyElement))
                                _apiKey = apiKeyElement.GetString();
                            if (rawgApi.TryGetProperty("BaseUrl", out var baseUrlElement))
                                _baseUrl = baseUrlElement.GetString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load configuration: {ex.Message}");
            }

            // Fallback to default values if configuration is not found
            _apiKey = _apiKey ?? "c36c04a598ad4131a503367c9fb5ae7b";
            _baseUrl = _baseUrl ?? "https://api.rawg.io/api/";
        }

        // Searches for games by name (used in the Search page)
        public async Task<List<Game>> SearchGamesAsync(string query)
        {
            // Return an empty list if the query is null or empty
            if (string.IsNullOrWhiteSpace(query))
                return new List<Game>();

            try
            {
                // Build the full request URL
                var url = $"{_baseUrl}games?key={_apiKey}&search={query}";

                // Send request and deserialize JSON into GameResponse
                var response = await _httpClient.GetFromJsonAsync<GameResponse>(url);

                // Return results if available, otherwise return an empty list
                return response?.Results ?? new List<Game>();
            }
            catch (Exception ex)
            {
                // Handle errors such as network issues or API failures
                Console.WriteLine($"เกิดข้อผิดพลาดในการดึงข้อมูล: {ex.Message}");
                return new List<Game>();
            }
        }
    }
}