using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Task
    {
        public int Id { get; set; } 

        [Required]
        public string Title { get; set; } 

        [Required]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; } // Esto ahora es obligatorio

        public User User { get; set; }
    }
}