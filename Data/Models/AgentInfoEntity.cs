using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fong.Data.Models {
    [Table("AgentInfo")]
    public class AgentInfoEntity {
        [Key]
        public int Id { get; set; }
        
        [StringLength(15)] // IP address length
        public string? Ip { get; set; }
        
        [StringLength(100)]
        public string? ModelName { get; set; }
        
        [StringLength(20)]
        public string? State { get; set; }
        
        [StringLength(50)]
        public string? AgentId { get; set; }
        
        [StringLength(200)]
        public string? FriendlyName { get; set; }
        
        [StringLength(100)]
        public string? DeviceType { get; set; }
        
        [StringLength(100)]
        public string? Manufacturer { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}