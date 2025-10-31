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

        [HttpGet("seasons")]
        public async Task<IActionResult> GetSeasons()
        {
            try
            {
                var seasons = await _blizzardService.GetPvpSeasonIndexAsync();

                if (seasons == null)
                    return NotFound("No seasons found");

                return Ok(seasons);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
