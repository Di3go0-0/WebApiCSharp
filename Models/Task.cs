using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }

        public User User { get; set; } = new User();
    }
}