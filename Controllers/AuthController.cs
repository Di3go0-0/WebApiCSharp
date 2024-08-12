using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = _authService.Register(user);
            if (result == "Email already exists.")
            {
                return BadRequest(result);
            }

            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            if (loginUser.Email == null || loginUser.Password == null)
            {
                return BadRequest("Invalid login data.");
            }

            var (token, error) = _authService.Login(loginUser);
            if (error != null)
            {
                return Unauthorized(error);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("token", token, cookieOptions);

            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(-1)
            };

            Response.Cookies.Append("token", "", cookieOptions);

            return Ok(new { Message = "Logged out successfully." });
        }
    }
}