using Fong.Contexts;
using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Device = Fong.Models.Database.Device;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Devices : ControllerBase {
        private readonly AppDbContext _context;

        public Devices(AppDbContext context) {
            _context = context;
        }

        // GET api/devices
        [HttpGet]
        public async Task<ActionResult<List<Device>>> GetAllDevices() {
            return await _context.Devices.ToListAsync();
        }

        // GET api/devices/online
        [HttpGet("online")]
        public async Task<ActionResult<List<Device>>> GetOnlineDevices() {
            var devices = await _context.Devices.ToListAsync();
            var onlineDevices = devices.Where(d => d.State == 1).ToList();
            return Ok(onlineDevices);
        }
    }
}