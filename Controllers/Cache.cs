using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Cache : ControllerBase {
        private readonly DataStorageService _dataStorageService;

        public Cache(DataStorageService dataStorageService) {
            _dataStorageService = dataStorageService;
        }

        // POST api/cache/refresh
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshCache() {
            try {
                await _dataStorageService.RefreshAllDataAsync();
                return Ok(new { message = "Cache refreshed successfully", timestamp = DateTime.UtcNow });
            } catch (Exception ex) {
                return StatusCode(500, new { error = "Failed to refresh cache", details = ex.Message });
            }
        }
    }
}