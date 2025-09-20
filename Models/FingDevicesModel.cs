namespace Fong.Models;

public class FingApiResponse {
    public string NetworkId { get; set; } = string.Empty;
    public List<FingDevice> Devices { get; set; } = new();
}

public class FingDevice {
    public string Mac { get; set; } = string.Empty;
    public List<string> Ip { get; set; } = []; // IP can have multiple values
    public string State { get; set; } = string.Empty; // "UP" or "DOWN"
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public DateTime First_Seen { get; set; }
    public DateTime? Last_Changed { get; set; }
}