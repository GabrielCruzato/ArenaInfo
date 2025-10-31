using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArenaInfo.Services;

namespace ArenaInfo.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly BlizzardService _blizzardService;

        public TestController(BlizzardService blizzardService)
        {
            _blizzardService = blizzardService;
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            var token = await _blizzardService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
                return StatusCode(500, "Failed to get token. Check console logs for details.");

            return Ok(new { access_token = token });
        }
    }
}
