using Microsoft.EntityFrameworkCore;
using Fong.Data.Models;

namespace Fong.Data.Repository {
    public class AgentInfoRepository : IAgentInfoRepository {
        private readonly FongDbContext _context;
        
        public AgentInfoRepository(FongDbContext context) {
            _context = context;
        }
        
        public async Task<AgentInfoEntity?> GetLatestAsync() {
            return await _context.AgentInfo
                .OrderByDescending(a => a.UpdatedAt)
                .FirstOrDefaultAsync();
        }
        
        public async Task<AgentInfoEntity> AddAsync(AgentInfoEntity agentInfo) {
            agentInfo.CreatedAt = DateTime.UtcNow;
            agentInfo.UpdatedAt = DateTime.UtcNow;
            
            _context.AgentInfo.Add(agentInfo);
            await _context.SaveChangesAsync();
            return agentInfo;
        }
        
        public async Task<AgentInfoEntity> UpdateAsync(AgentInfoEntity agentInfo) {
            agentInfo.UpdatedAt = DateTime.UtcNow;
            
            _context.AgentInfo.Update(agentInfo);
            await _context.SaveChangesAsync();
            return agentInfo;
        }
        
        public async Task DeleteAsync(int id) {
            var agentInfo = await _context.AgentInfo.FindAsync(id);
            if (agentInfo != null) {
                _context.AgentInfo.Remove(agentInfo);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }
        
        public async Task InsertOrUpdateLatestAsync(AgentInfoEntity agentInfo) {
            // Remove all existing agent info and add new one (we only keep the latest)
            var existingAgentInfo = await _context.AgentInfo.ToListAsync();
            if (existingAgentInfo.Any()) {
                _context.AgentInfo.RemoveRange(existingAgentInfo);
            }
            
            agentInfo.CreatedAt = DateTime.UtcNow;
            agentInfo.UpdatedAt = DateTime.UtcNow;
            _context.AgentInfo.Add(agentInfo);
            
            await _context.SaveChangesAsync();
        }
    }
}