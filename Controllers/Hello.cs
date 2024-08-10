using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Hello : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hola mundo");
        }
    }
}