using Microsoft.EntityFrameworkCore;
using Fong.Data.Models;

namespace Fong.Data {
    public class FongDbContext : DbContext {
        public FongDbContext(DbContextOptions<FongDbContext> options) : base(options) {
        }
        
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<AgentInfoEntity> AgentInfo { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            
            // Configure unique constraints
            modelBuilder.Entity<DeviceEntity>()
                .HasIndex(d => d.Mac)
                .IsUnique();
                
            modelBuilder.Entity<ContactEntity>()
                .HasIndex(c => c.ContactId)
                .IsUnique();
        }
    }
}