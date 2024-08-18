using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.DTOs;
using WebApi.Models;
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
        public  async Task<IActionResult> GetTask()
        {   
            List<TaskModel> tasks = await _taskService.GetTask();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO taskDTO)
        {
            if (string.IsNullOrEmpty(taskDTO.Title) || string.IsNullOrEmpty(taskDTO.Description))
            {
                return BadRequest("The Title and Description fields are required.");
            }

            string result =  await _taskService.CreateTask(taskDTO);

            return Ok(result);
        }
    }
}