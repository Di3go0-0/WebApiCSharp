using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (result == "Email already exists.")
            {
                return BadRequest(new { Message = result });
            }

            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto, Response);

            if (result == "Logged in successfully.")
            {
                return Ok(new { Message = result });
            }

            return Unauthorized(new { Message = result });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = _authService.Logout(Response);
            if (result == "Logged out successfully.")
            {
                return Ok(new { Message = result });
            }

            return StatusCode(500, new { Message = result });
        }
    }
}
