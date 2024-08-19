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
        private readonly ITaskRepository  _taskRepository;

        public TaskController(AuthenticationService authenticationService, ITaskRepository taskRepository)
        {
            _authenticationService = authenticationService;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                int currentUserId = _authenticationService.AuthenticateUser();
                List<TaskModel> tasks = await _taskRepository.GetTasksAsync(currentUserId);
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO taskDTO)
        {
            
            int currentUserId = _authenticationService.AuthenticateUser();

            TaskModel result = await _taskRepository.CreateTaskAsync(taskDTO, currentUserId);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByID(string id)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            int taskId = int.Parse(id);

            TaskModel task = await _taskRepository.GetTaskAsync(currentUserId, taskId);

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            int taskId = int.Parse(id);

            string Message = await _taskRepository.DeleteTaskAsync(taskId, currentUserId);

            return Ok(Message);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskDTO task)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            int taskId = int.Parse(id);

            TaskModel taskUpdated = await _taskRepository.UpdateTaskAsync(taskId, task, currentUserId);

            return Ok(taskUpdated);
        }

    }
}