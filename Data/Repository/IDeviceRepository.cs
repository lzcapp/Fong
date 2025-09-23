using Fong.Data.Models;

namespace Fong.Data.Repository {
    public interface IDeviceRepository {
        Task<IEnumerable<DeviceEntity>> GetAllAsync();
        Task<IEnumerable<DeviceEntity>> GetActiveAsync();
        Task<DeviceEntity?> GetByMacAsync(string mac);
        Task<DeviceEntity> AddAsync(DeviceEntity device);
        Task<DeviceEntity> UpdateAsync(DeviceEntity device);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string mac);
        Task SaveChangesAsync();
        Task BulkInsertOrUpdateAsync(IEnumerable<DeviceEntity> devices);
    }
}