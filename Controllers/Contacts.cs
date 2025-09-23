using System.Net;
using Fong.Models;
using Fong.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fong.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Contacts : ControllerBase {
        private readonly FingService _fingService;

        public Contacts(FingService fingService) {
            _fingService = fingService;
        }

        // GET api/contacts
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAllContacts() {
            try {
                var devices = await _fingService.GetContactsAsync();
                return Ok(devices);
            } catch (HttpRequestException e) {
                if (e.StatusCode == HttpStatusCode.Unauthorized) {
                    return Unauthorized();
                }
                return Forbid();
            } catch (Exception e) {
                Console.WriteLine(e);
                return Forbid();
            }
        }
    }
}