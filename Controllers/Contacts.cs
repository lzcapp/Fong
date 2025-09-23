using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Contacts : ControllerBase {
        private readonly DataStorageService _dataStorageService;

        public Contacts(DataStorageService dataStorageService) {
            _dataStorageService = dataStorageService;
        }

        // GET api/contacts
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAllContacts([FromQuery] bool useCache = true, [FromQuery] bool refresh = false) {
            var contacts = await _dataStorageService.GetContactsAsync(useCache, refresh);
            return Ok(contacts);
        }
    }
}