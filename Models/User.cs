using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}

}