using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Propiedad de navegación para las tareas
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}