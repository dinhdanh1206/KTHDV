using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupermarketController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello World");
        }
    }
}
