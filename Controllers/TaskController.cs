using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
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
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                List<TaskModel> tasks = await _taskService.GetTasksAsync();
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tasks.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO taskDTO)
        {
            try
            {
                string response = await _taskService.CreateTaskAsync(taskDTO);
                return Ok(new { message = response });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the task.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            try
            {
                if (!int.TryParse(id, out int taskId))
                {
                    return BadRequest(new { message = "Invalid task ID format." });
                }

                TaskModel? task = await _taskService.GetTaskAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { message = "Task not found or not associated with the user." });
                }

                return Ok(new { message = "Task found.", data = task });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the task.", error = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskDTO task)
        {
            try
            {
                if (!int.TryParse(id, out int taskId))
                {
                    return BadRequest(new { message = "Invalid task ID format." });
                }

                TaskModel? taskUpdated = await _taskService.UpdateTaskAsync(taskId, task);
                if (taskUpdated == null)
                {
                    return NotFound(new { message = "Task not found or not associated with the user." });
                }

                return Ok(new { message = "Task updated.", data = taskUpdated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the task.", error = ex.Message });
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {
            try
            {
                if (!int.TryParse(taskId, out int parsedTaskId))
                {
                    return BadRequest(new { message = "Invalid task ID format." });
                }

                string response = await _taskService.DeleteTaskAsync(parsedTaskId);
                if (response.Contains("successfully"))
                {
                    return Ok(new { message = response });
                }
                return NotFound(new { message = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the task.", error = ex.Message });
            }
        }
    }
}
