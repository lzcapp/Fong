using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fong.Data.Models {
    [Table("Contacts")]
    public class ContactEntity {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ContactId { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string DisplayName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string ContactType { get; set; } = string.Empty;
        
        public string? PictureImageData { get; set; }
        
        [StringLength(500)]
        public string? PictureUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}