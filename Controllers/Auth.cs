using Microsoft.AspNetCore.Mvc;
using WebApi.Context;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Auth : ControllerBase
    {
        private readonly AppDbContext _context;

        public Auth(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Check if the email already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("Email already exists.");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok("Auth");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var response = new { Message = "Login" };
            return Ok(response);
        }
    }
}