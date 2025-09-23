using System.Text.Json.Serialization;

namespace Fong.Models {
    public class DeviceResponse {
        [JsonPropertyName("networkId")] public string? NetworkId { get; set; }

        [JsonPropertyName("devices")] public List<Device> Devices { get; set; } = [];
    }
    
    public class Device {
        [JsonPropertyName("mac")] public string Mac { get; set; } = string.Empty;

        [JsonPropertyName("ip")] public List<string> Ip { get; set; } = new List<string>();

        [JsonPropertyName("state")] public string State { get; set; } = string.Empty;

        [JsonPropertyName("name")] public string? Name { get; set; }

        [JsonPropertyName("type")] public string? Type { get; set; }

        [JsonPropertyName("make")] public string? Make { get; set; }

        [JsonPropertyName("model")] public string? Model { get; set; }

        [JsonPropertyName("contactId")] public string? ContactId { get; set; }

        [JsonPropertyName("first_seen")] public string? FirstSeen { get; set; }

        [JsonPropertyName("last_changed")] public string? LastChanged { get; set; }

        [JsonIgnore] public bool Active => State == "UP";
    }
}