using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fong.Models.Database {
    public class Device {
        [Key][Column("id")] // Primary key
        public long Id { get; set; }
        
        [Column("mac")]
        public string Mac { get; set; } = string.Empty;
        
        [Required][Column("ip")]
        public string? Ip { get; set; }

        [Required][Column("state")]
        public int State { get; set; } = -1;
        
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        
        [Column("type")]
        public string Type { get; set; } = string.Empty;
        
        [Column("vendor")]
        public string Vendor { get; set; } = string.Empty;
        
        [Column("model")]
        public string Model { get; set; } = string.Empty;
        
        [Column("contact_id")]
        public string ContactId { get; set; } = string.Empty;
        
        [Column("first_seen")]
        public long? FirstSeen { get; set; }
        
        
        [Column("last_changed")]
        public long? LastChanged { get; set; }
    }
}