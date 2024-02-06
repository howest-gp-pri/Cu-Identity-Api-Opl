using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pri.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "OnlyCitizensFromBruges")]
        public async Task<IActionResult> Get()
        {
            return Ok("Welcome citizen from Bruges");
        }

    }
}
