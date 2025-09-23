using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AgentInfo : ControllerBase {
        private readonly DataStorageService _dataStorageService;

        public AgentInfo(DataStorageService dataStorageService) {
            _dataStorageService = dataStorageService;
        }

        // GET api/agentinfo
        [HttpGet]
        public async Task<ActionResult<AgentInfo?>> GetAgentInfo([FromQuery] bool useCache = true, [FromQuery] bool refresh = false) {
            var agentInfo = await _dataStorageService.GetAgentInfoAsync(useCache, refresh);
            return Ok(agentInfo);
        }
    }
}