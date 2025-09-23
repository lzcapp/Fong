using Fong.Data.Models;

namespace Fong.Data.Repository {
    public interface IAgentInfoRepository {
        Task<AgentInfoEntity?> GetLatestAsync();
        Task<AgentInfoEntity> AddAsync(AgentInfoEntity agentInfo);
        Task<AgentInfoEntity> UpdateAsync(AgentInfoEntity agentInfo);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task InsertOrUpdateLatestAsync(AgentInfoEntity agentInfo);
    }
}