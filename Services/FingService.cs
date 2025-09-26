using Fong.Configs;
using Fong.Contexts;
using System.Text.Json;
using Fong.Helpers;
using Fong.Models.Fing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fong.Services {
    public class FingService {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FingService> _logger;
        private readonly string _fingApiHost, _fingApiPort, _fingApiKey, _fingAgentPort = "44444";

        public FingService(AppDbContext context, HttpClient httpClient, IConfiguration config, IOptions<FingApiSettings> settings, ILogger<FingService> logger) {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            if (AppHelper.IsInDocker) {
                _fingApiHost = config["FING_API_HOST"] ?? string.Empty;
                _fingApiPort = config["FING_API_PORT"] ?? string.Empty;
                _fingApiKey = config["FING_API_KEY"] ?? string.Empty;
            } else {
                _fingApiHost = settings.Value.ApiHost;
                _fingApiPort = settings.Value.ApiPort;
                _fingApiKey = settings.Value.ApiKey;
            }

            if (string.IsNullOrWhiteSpace(_fingApiHost) || string.IsNullOrWhiteSpace(_fingApiPort) || string.IsNullOrWhiteSpace(_fingApiKey)) {
                throw new ArgumentException();
            }
        }

        private string BuildFingApiUrl(string endpoint) {
            var baseUrl = endpoint.Equals("agent", StringComparison.InvariantCultureIgnoreCase) ? $"{_fingApiHost}:{_fingAgentPort}/" : $"{_fingApiHost}:{_fingApiPort}/1/{endpoint}?auth={_fingApiKey}";
            return baseUrl;
        }

        public async Task SyncDevicesAsync() {
            try {
                var url = BuildFingApiUrl("devices");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };

                var deviceResponse = JsonSerializer.Deserialize<DeviceResponse>(json, options);
                var devices = deviceResponse?.Devices ?? [];
                if (devices is { Count: > 0 }) {
                    foreach (var device in devices) {
                        var mac = device.Mac;
                        var existing = await _context.Devices.FirstOrDefaultAsync(d => d.Mac == mac);

                        var name = device.Name ?? string.Empty;
                        var state = device.Active ? 1 : 0;
                        var ip = string.Join(",", device.Ip);
                        var type = device.Type ?? string.Empty;
                        var vendor = device.Make ?? string.Empty;
                        var model = device.Model ?? string.Empty;
                        var contactId = device.ContactId ?? string.Empty;
                        long? firstSeen = null;
                        if (DateTime.TryParse(device.FirstSeen, out var dateFirstSeen)) {
                            firstSeen = new DateTimeOffset(dateFirstSeen.ToUniversalTime()).ToUnixTimeMilliseconds();
                        }
                        long? lastChanged = null;
                        if (DateTime.TryParse(device.LastChanged, out var dateLastChanged)) {
                            lastChanged = new DateTimeOffset(dateLastChanged.ToUniversalTime()).ToUnixTimeMilliseconds();
                        }
                        
                        if (existing != null) {
                            existing.Ip = ip;
                            existing.State = state;
                            existing.Type = type;
                            existing.Vendor = vendor;
                            existing.Model = model;
                            existing.ContactId = contactId;
                            existing.FirstSeen = firstSeen ?? existing.FirstSeen;
                            existing.LastChanged = lastChanged ?? existing.LastChanged;
                        } else {
                            var dbDevice = new Fong.Models.Database.Device {
                                Id = AppHelper.GenerateHash(mac),
                                Mac = mac,
                                Ip = ip,
                                State = state,
                                Name = name,
                                Type = type,
                                Vendor = vendor,
                                Model = model,
                                ContactId = contactId,
                                FirstSeen = firstSeen,
                                LastChanged = lastChanged
                            };
                            _context.Devices.Add(dbDevice);
                        }
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Device sync complete. Total devices: {Count}", devices.Count);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Fing API data");
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
                throw;
            }
        }
    
        public async Task<AgentInfo?> GetAgentInfoAsync() {
            try {
                var url = BuildFingApiUrl("agent");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var xml = await response.Content.ReadAsStringAsync();

                var agentInfo = new AgentInfo(xml);
                return agentInfo;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error fetching Fing API data");
                throw;
            }
        }
    }
}