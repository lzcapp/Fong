using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Devices : ControllerBase {
        private readonly DataStorageService _dataStorageService;

        public Devices(DataStorageService dataStorageService) {
            _dataStorageService = dataStorageService;
        }

        // GET api/devices
        [HttpGet]
        public async Task<ActionResult<List<Device>>> GetAllDevices([FromQuery] bool useCache = true, [FromQuery] bool refresh = false) {
            var devices = await _dataStorageService.GetDevicesAsync(useCache, refresh);
            return Ok(devices);
        }

        // GET api/devices/online
        [HttpGet("online")]
        public async Task<ActionResult<List<Device>>> GetOnlineDevices([FromQuery] bool useCache = true, [FromQuery] bool refresh = false) {
            var devices = await _dataStorageService.GetActiveDevicesAsync(useCache, refresh);
            return Ok(devices);
        }
    }
}