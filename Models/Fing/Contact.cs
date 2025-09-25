using System.Text.Json.Serialization;

namespace Fong.Models.Fing {
    public class ContactResponse {
        [JsonPropertyName("contacts")] public List<Contact> Contacts { get; set; } = [];
    }

    public class Contact {
        [JsonPropertyName("info")] public ContactInfo? Info { get; set; }
    }

    public class ContactInfo {
        [JsonPropertyName("contactId")] public string ContactId { get; set; } = string.Empty;

        [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("contactType")] public string ContactType { get; set; } = string.Empty;

        [JsonPropertyName("pictureImageData")] public string? PictureImageData { get; set; }

        [JsonPropertyName("pictureUrl")] public string? PictureUrl { get; set; }
    }
}