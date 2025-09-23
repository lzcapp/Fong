using Microsoft.EntityFrameworkCore;
using Fong.Data.Models;

namespace Fong.Data.Repository {
    public class ContactRepository : IContactRepository {
        private readonly FongDbContext _context;
        
        public ContactRepository(FongDbContext context) {
            _context = context;
        }
        
        public async Task<IEnumerable<ContactEntity>> GetAllAsync() {
            return await _context.Contacts.ToListAsync();
        }
        
        public async Task<ContactEntity?> GetByContactIdAsync(string contactId) {
            return await _context.Contacts
                .FirstOrDefaultAsync(c => c.ContactId == contactId);
        }
        
        public async Task<ContactEntity> AddAsync(ContactEntity contact) {
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;
            
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }
        
        public async Task<ContactEntity> UpdateAsync(ContactEntity contact) {
            contact.UpdatedAt = DateTime.UtcNow;
            
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return contact;
        }
        
        public async Task DeleteAsync(int id) {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null) {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> ExistsAsync(string contactId) {
            return await _context.Contacts.AnyAsync(c => c.ContactId == contactId);
        }
        
        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }
        
        public async Task BulkInsertOrUpdateAsync(IEnumerable<ContactEntity> contacts) {
            foreach (var contact in contacts) {
                var existing = await _context.Contacts
                    .FirstOrDefaultAsync(c => c.ContactId == contact.ContactId);
                    
                if (existing != null) {
                    // Update existing contact
                    existing.DisplayName = contact.DisplayName;
                    existing.ContactType = contact.ContactType;
                    existing.PictureImageData = contact.PictureImageData;
                    existing.PictureUrl = contact.PictureUrl;
                    existing.UpdatedAt = DateTime.UtcNow;
                } else {
                    // Add new contact
                    contact.CreatedAt = DateTime.UtcNow;
                    contact.UpdatedAt = DateTime.UtcNow;
                    _context.Contacts.Add(contact);
                }
            }
            
            await _context.SaveChangesAsync();
        }
    }
}