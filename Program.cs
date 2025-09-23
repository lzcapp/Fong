using Fong.Configs;
using Fong.Services;
using Fong.Data;
using Fong.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fong {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<FingService>();
            
            builder.Services.Configure<FingApiSettings>(builder.Configuration.GetSection("FingApiSettings"));

            // Add Entity Framework and SQLite
            builder.Services.AddDbContext<FongDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=fong.db"));

            // Register repositories
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IAgentInfoRepository, AgentInfoRepository>();
            
            // Register data storage service
            builder.Services.AddScoped<DataStorageService>();

            var app = builder.Build();

            // Ensure database is created
            using (var scope = app.Services.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<FongDbContext>();
                context.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}