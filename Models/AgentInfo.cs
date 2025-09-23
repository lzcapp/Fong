using System.Text.Json.Serialization;

namespace Fong.Models {
    public class AgentInfo {
        [JsonPropertyName("ip")] public string? Ip { get; set; }

        [JsonPropertyName("model_name")] public string? ModelName { get; set; }

        [JsonPropertyName("state")] public string? State { get; set; }

        [JsonPropertyName("agent_id")] public string? AgentId { get; set; }

        [JsonPropertyName("friendly_name")] public string? FriendlyName { get; set; }

        [JsonPropertyName("device_type")] public string? DeviceType { get; set; }

        [JsonPropertyName("manufacturer")] public string? Manufacturer { get; set; }
    }
}