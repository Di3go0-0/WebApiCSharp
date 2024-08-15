using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        // Hacer que Tasks sea opcional
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}