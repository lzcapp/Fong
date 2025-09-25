using System.ComponentModel.DataAnnotations;

namespace Fong.Models.Database {
    public class Device {
        [Key] // Primary key
        public long Id { get; set; }

        public string Mac { get; set; } = string.Empty;
        
        [Required]
        public string? Ip { get; set; }

        [Required]
        public int State { get; set; } = -1;
        
        public string Name { get; set; } = string.Empty;
        
        public string Type { get; set; } = string.Empty;
        
        public string Vendor { get; set; } = string.Empty;
        
        public string Model { get; set; } = string.Empty;
        
        public string ContactId { get; set; } = string.Empty;
        
        public long? FirstSeen { get; set; }
        
        public long? LastChanged { get; set; }
    }
}