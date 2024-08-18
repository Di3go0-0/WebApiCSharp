using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class TaskModel
{
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; } // Esto ahora es obligatorio

        [JsonIgnore]
        public User? User { get; set; }
    }
}