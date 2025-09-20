using System.Text.Json;
using Fong.Models;

namespace Fong.Services;

public class FingService {
    private readonly HttpClient _httpClient;
    private readonly ILogger<FingService> _logger;
    private readonly string _fingApiHost;
    private readonly string _fingApiKey;

    public FingService(HttpClient httpClient, IConfiguration config, ILogger<FingService> logger) {
        _httpClient = httpClient;
        _logger = logger;
        _fingApiHost = config["FING_API_HOST"] ?? throw new ArgumentNullException("FING_API_HOST is missing!");
        _fingApiKey = config["FING_API_KEY"] ?? throw new ArgumentNullException("FING_API_KEY is missing!");
    }

    private string BuildFingApiUrl(string endpoint, string? extraQuery = null) {
        var baseUrl = $"{_fingApiHost}/1/{endpoint}?auth={_fingApiKey}";
        return string.IsNullOrWhiteSpace(extraQuery) ? baseUrl : $"{baseUrl}&{extraQuery}";
    }

    public async Task<List<FingDevice>> GetDevicesAsync() {
        try {
            var url = BuildFingApiUrl("devices");
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var fingResponse = JsonSerializer.Deserialize<FingApiResponse>(json, options);
            return fingResponse?.Devices ?? [];
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error fetching Fing API data");
            return [];
        }
    }
}