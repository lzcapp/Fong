using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;
using Device = Fong.Models.Database.Device;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AgentInfo : ControllerBase {
        private readonly FingService _fingService;

        public AgentInfo(FingService fingService) {
            _fingService = fingService;
        }

        // GET api/agentinfo
        [HttpGet]
        public async Task<ActionResult<List<Device>>> GetAgentInfo() {
            var devices = await _fingService.GetAgentInfoAsync();
            return Ok(devices);
        }
    }
}