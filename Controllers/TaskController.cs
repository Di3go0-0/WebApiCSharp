using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Interfaces;
// using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly TaskService _taskService;
        private readonly ITaskRepository _taskRepository;

        public TaskController(AuthenticationService authenticationService, ITaskRepository taskRepository, TaskService taskService)
        {
            _authenticationService = authenticationService;
            _taskRepository = taskRepository;
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
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO taskDTO)
        {

            try
            {
                int currentUserId = _authenticationService.AuthenticateUser();

                string response = await _taskRepository.CreateTaskAsync(taskDTO, currentUserId);

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
            catch (System.Exception)
            {

                return StatusCode(500, new { message = "Unexpected error." });
            }


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByID(string id)
        {
            try
            {
                int taskId = int.Parse(id);

                TaskModel? task = await _taskService.GetTaskAsync(taskId);
                if (task == null)
                {
                    return NotFound(new { message = "Task not found." });
                }

                return Ok(new
                {
                    Message = "Task found.",
                    data = task
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { message = "Task not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Task is not associated with the user." });
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "Unexpected error." });
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

                return Ok(new
                {
                    Message = "Task updated.",
                    Data = taskUpdated
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Task is not associated with the user." });
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { message = "Task not found." });
            }
            catch (System.Exception ex)
            {
                // Puedes loggear el error aquí si es necesario
                return StatusCode(500, new { message = "Unexpected error.", error = ex.Message });
            }
        }


        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {
            string reponse = await _taskService.DeleteTaskAsync(int.Parse(taskId));
            if (reponse.Contains("Task deleted successfully."))
            {
                return Ok(new { message = reponse });
            }
            return NotFound(new { message = reponse });
        }

    }
}