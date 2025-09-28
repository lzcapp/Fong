using Fong.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        public Task<ActionResult<List<Device>>> GetAllDevices() {
            return Task.FromResult<ActionResult<List<Device>>>(_context.Devices.AsEnumerable()
                .OrderBy(d => d.State != 1 ? d.State == 0 ? 1 : d.State == -1 ? 2 : 3 : 0).ThenBy(d => Ip2Uint(d.Ip))
                .ThenBy(d => d.Mac).ToList());
        }

        private static long Ip2Uint(string? dIp) {
            try {
                var ip = dIp.Split(",")[0];
                var bytes = IPAddress.Parse(ip).GetAddressBytes();
                if (BitConverter.IsLittleEndian) {
                    Array.Reverse(bytes);
                }
                return BitConverter.ToUInt32(bytes, 0);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return long.MaxValue;
            }
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