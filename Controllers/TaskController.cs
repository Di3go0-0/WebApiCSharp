using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.DTOs;
// using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] CreateTaskDTO taskDTO)
        {
            if (string.IsNullOrEmpty(taskDTO.Title) || string.IsNullOrEmpty(taskDTO.Description))
            {
                return BadRequest("The Title and Description fields are required.");
            }

            var result = _taskService.CreateTask(taskDTO);

            return Ok(result);
        }
    }
}