using Microsoft.EntityFrameworkCore;
using Fong.Data.Models;

namespace Fong.Data.Repository {
    public class DeviceRepository : IDeviceRepository {
        private readonly FongDbContext _context;
        
        public DeviceRepository(FongDbContext context) {
            _context = context;
        }
        
        public async Task<IEnumerable<DeviceEntity>> GetAllAsync() {
            return await _context.Devices.ToListAsync();
        }
        
        public async Task<IEnumerable<DeviceEntity>> GetActiveAsync() {
            return await _context.Devices
                .Where(d => d.State == "UP")
                .ToListAsync();
        }
        
        public async Task<DeviceEntity?> GetByMacAsync(string mac) {
            return await _context.Devices
                .FirstOrDefaultAsync(d => d.Mac == mac);
        }
        
        public async Task<DeviceEntity> AddAsync(DeviceEntity device) {
            device.CreatedAt = DateTime.UtcNow;
            device.UpdatedAt = DateTime.UtcNow;
            
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }
        
        public async Task<DeviceEntity> UpdateAsync(DeviceEntity device) {
            device.UpdatedAt = DateTime.UtcNow;
            
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
            return device;
        }
        
        public async Task DeleteAsync(int id) {
            var device = await _context.Devices.FindAsync(id);
            if (device != null) {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> ExistsAsync(string mac) {
            return await _context.Devices.AnyAsync(d => d.Mac == mac);
        }
        
        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }
        
        public async Task BulkInsertOrUpdateAsync(IEnumerable<DeviceEntity> devices) {
            foreach (var device in devices) {
                var existing = await _context.Devices
                    .FirstOrDefaultAsync(d => d.Mac == device.Mac);
                    
                if (existing != null) {
                    // Update existing device
                    existing.Ip = device.Ip;
                    existing.State = device.State;
                    existing.Name = device.Name;
                    existing.Type = device.Type;
                    existing.Make = device.Make;
                    existing.Model = device.Model;
                    existing.ContactId = device.ContactId;
                    existing.FirstSeen = device.FirstSeen;
                    existing.LastChanged = device.LastChanged;
                    existing.UpdatedAt = DateTime.UtcNow;
                } else {
                    // Add new device
                    device.CreatedAt = DateTime.UtcNow;
                    device.UpdatedAt = DateTime.UtcNow;
                    _context.Devices.Add(device);
                }
            }
            
            await _context.SaveChangesAsync();
        }
    }
}