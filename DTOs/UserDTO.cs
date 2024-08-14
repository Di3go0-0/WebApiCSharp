using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        [EmailAddress]

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
