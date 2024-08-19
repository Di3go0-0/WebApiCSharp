using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApi.DTOs
{
    public class CreateTaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateTaskDTO
    {        
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}