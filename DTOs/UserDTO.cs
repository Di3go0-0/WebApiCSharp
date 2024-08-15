using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Username { get; set; } 
        public string? Email { get; set; }
    }
}
