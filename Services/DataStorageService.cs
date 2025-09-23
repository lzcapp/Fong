using Fong.Data.Repository;
using Fong.Extensions;
using Fong.Models;

namespace Fong.Services {
    public class DataStorageService {
        private readonly FingService _fingService;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAgentInfoRepository _agentInfoRepository;
        private readonly ILogger<DataStorageService> _logger;
        
        public DataStorageService(
            FingService fingService,
            IDeviceRepository deviceRepository,
            IContactRepository contactRepository,
            IAgentInfoRepository agentInfoRepository,
            ILogger<DataStorageService> logger) {
            _fingService = fingService;
            _deviceRepository = deviceRepository;
            _contactRepository = contactRepository;
            _agentInfoRepository = agentInfoRepository;
            _logger = logger;
        }
        
        public async Task<List<Device>> GetDevicesAsync(bool useCache = true, bool refreshCache = false) {
            if (useCache && !refreshCache) {
                var cachedDevices = await _deviceRepository.GetAllAsync();
                if (cachedDevices.Any()) {
                    _logger.LogInformation("Returning {Count} devices from cache", cachedDevices.Count());
                    return cachedDevices.Select(d => d.ToModel()).ToList();
                }
            }
            
            // Fetch from API
            _logger.LogInformation("Fetching devices from API");
            var apiDevices = await _fingService.GetDevicesAsync();
            
            if (apiDevices.Any()) {
                // Store in database
                var deviceEntities = apiDevices.Select(d => d.ToEntity());
                await _deviceRepository.BulkInsertOrUpdateAsync(deviceEntities);
                _logger.LogInformation("Stored {Count} devices in database", apiDevices.Count);
            }
            
            return apiDevices;
        }
        
        public async Task<List<Device>> GetActiveDevicesAsync(bool useCache = true, bool refreshCache = false) {
            if (useCache && !refreshCache) {
                var cachedDevices = await _deviceRepository.GetActiveAsync();
                if (cachedDevices.Any()) {
                    _logger.LogInformation("Returning {Count} active devices from cache", cachedDevices.Count());
                    return cachedDevices.Select(d => d.ToModel()).ToList();
                }
            }
            
            // Get all devices and filter active ones
            var allDevices = await GetDevicesAsync(useCache, refreshCache);
            return allDevices.Where(d => d.Active).ToList();
        }
        
        public async Task<List<Contact>> GetContactsAsync(bool useCache = true, bool refreshCache = false) {
            if (useCache && !refreshCache) {
                var cachedContacts = await _contactRepository.GetAllAsync();
                if (cachedContacts.Any()) {
                    _logger.LogInformation("Returning {Count} contacts from cache", cachedContacts.Count());
                    return cachedContacts.Select(c => c.ToModel()).ToList();
                }
            }
            
            // Fetch from API
            _logger.LogInformation("Fetching contacts from API");
            var apiContacts = await _fingService.GetContactsAsync();
            
            if (apiContacts.Any()) {
                // Store in database
                var contactEntities = apiContacts.Select(c => c.ToEntity());
                await _contactRepository.BulkInsertOrUpdateAsync(contactEntities);
                _logger.LogInformation("Stored {Count} contacts in database", apiContacts.Count);
            }
            
            return apiContacts;
        }
        
        public async Task<AgentInfo?> GetAgentInfoAsync(bool useCache = true, bool refreshCache = false) {
            if (useCache && !refreshCache) {
                var cachedAgentInfo = await _agentInfoRepository.GetLatestAsync();
                if (cachedAgentInfo != null) {
                    _logger.LogInformation("Returning agent info from cache");
                    return cachedAgentInfo.ToModel();
                }
            }
            
            // Fetch from API
            _logger.LogInformation("Fetching agent info from API");
            var apiAgentInfo = await _fingService.GetAgentInfoAsync();
            
            if (apiAgentInfo != null) {
                // Store in database
                var agentInfoEntity = apiAgentInfo.ToEntity();
                await _agentInfoRepository.InsertOrUpdateLatestAsync(agentInfoEntity);
                _logger.LogInformation("Stored agent info in database");
            }
            
            return apiAgentInfo;
        }
        
        public async Task RefreshAllDataAsync() {
            _logger.LogInformation("Refreshing all data from API");
            
            try {
                await Task.WhenAll(
                    GetDevicesAsync(useCache: false, refreshCache: true),
                    GetContactsAsync(useCache: false, refreshCache: true),
                    GetAgentInfoAsync(useCache: false, refreshCache: true)
                );
                
                _logger.LogInformation("Successfully refreshed all data");
            } catch (Exception ex) {
                _logger.LogError(ex, "Error refreshing all data");
                throw;
            }
        }
    }
}