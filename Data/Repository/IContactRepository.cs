using Fong.Data.Models;

namespace Fong.Data.Repository {
    public interface IContactRepository {
        Task<IEnumerable<ContactEntity>> GetAllAsync();
        Task<ContactEntity?> GetByContactIdAsync(string contactId);
        Task<ContactEntity> AddAsync(ContactEntity contact);
        Task<ContactEntity> UpdateAsync(ContactEntity contact);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string contactId);
        Task SaveChangesAsync();
        Task BulkInsertOrUpdateAsync(IEnumerable<ContactEntity> contacts);
    }
}