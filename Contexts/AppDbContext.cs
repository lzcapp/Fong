using Fong.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Fong.Contexts {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Make MacAddress unique
            modelBuilder.Entity<Device>()
                .HasIndex(d => d.Id)
                .IsUnique();
        }
    }
}