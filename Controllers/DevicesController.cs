using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase {
        private readonly FingService _fingService;

        public DevicesController(FingService fingService) {
            _fingService = fingService;
        }

        // GET api/devices
        [HttpGet]
        public async Task<ActionResult<List<Device>>> GetAllDevices() {
            var devices = await _fingService.GetDevicesAsync();
            return Ok(devices);
        }

        // GET api/devices/online
        [HttpGet("online")]
        public async Task<ActionResult<List<Device>>> GetOnlineDevices() {
            var devices = await _fingService.GetDevicesAsync();
            var onlineDevices = devices.Where(d => d.State.Equals("UP", StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(onlineDevices);
        }
    }
}