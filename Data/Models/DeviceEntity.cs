using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fong.Data.Models {
    [Table("Devices")]
    public class DeviceEntity {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(17)] // MAC address length
        public string Mac { get; set; } = string.Empty;
        
        public string? Ip { get; set; } // JSON serialized IP list
        
        [StringLength(10)]
        public string State { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Name { get; set; }
        
        [StringLength(100)]
        public string? Type { get; set; }
        
        [StringLength(100)]
        public string? Make { get; set; }
        
        [StringLength(100)]
        public string? Model { get; set; }
        
        [StringLength(50)]
        public string? ContactId { get; set; }
        
        public string? FirstSeen { get; set; }
        
        public string? LastChanged { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [NotMapped]
        public bool Active => State == "UP";
    }
}