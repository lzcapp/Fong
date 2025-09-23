using System.Text.Json;
using Fong.Models;

namespace Fong.Services {
    public class FingService {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FingService> _logger;
        private readonly string _fingApiHost, _fingApiPort, _fingApiKey, _fingAgentPort = "44444";

        public FingService(HttpClient httpClient, IConfiguration config, ILogger<FingService> logger) {
            _httpClient = httpClient;
            _logger = logger;
            _fingApiHost = config["FING_API_HOST"] ?? throw new ArgumentNullException("FING_API_HOST is missing!");
            _fingApiPort = config["FING_API_PORT"] ?? throw new ArgumentNullException("FING_API_PORT is missing!");
            _fingApiKey = config["FING_API_KEY"] ?? throw new ArgumentNullException("FING_API_KEY is missing!");
        }

        private string BuildFingApiUrl(string endpoint) {
            string baseUrl;
            if (!endpoint.Equals("agent", StringComparison.InvariantCultureIgnoreCase)) {
                baseUrl = $"{_fingApiHost}:{_fingApiPort}/1/{endpoint}?auth={_fingApiKey}";
            } else {
                baseUrl = $"{_fingApiHost}:{_fingAgentPort}/";
            }
            return baseUrl;
        }

        public async Task<List<Device>> GetDevicesAsync() {
            try {
                var url = BuildFingApiUrl("devices");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };

                var deviceResponse = JsonSerializer.Deserialize<DeviceResponse>(json, options);
                return deviceResponse?.Devices ?? [];
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Fing API data");
                return [];
            }
        }

        public async Task<List<Contact>> GetContactsAsync() {
            try {
                var url = BuildFingApiUrl("people");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };

                var contactResponse = JsonSerializer.Deserialize<ContactResponse>(json, options);
                return contactResponse?.Contacts ?? [];
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Fing API data");
                return [];
            }
        }
    
        public async Task<AgentInfo?> GetAgentInfoAsync() {
            try {
                var url = BuildFingApiUrl("agent");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };

                var agentInfo = JsonSerializer.Deserialize<AgentInfo>(json, options);
                return agentInfo;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Fing API data");
                return null;
            }
        }
    }
}