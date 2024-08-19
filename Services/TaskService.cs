using WebApi.Models;
using WebApi.DTOs;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class TaskService
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository, AuthenticationService authenticationService)
        {
            _taskRepository = taskRepository;
            _authenticationService = authenticationService;
        }

        public async Task<List<TaskModel>> GetTasksAsync()
        {
            int userId = _authenticationService.AuthenticateUser();
            return await _taskRepository.GetTasksAsync(userId);
        }

        public async Task<string> CreateTaskAsync(CreateTaskDTO taskDTO)
        {
            int userId = _authenticationService.AuthenticateUser();
            return await _taskRepository.CreateTaskAsync(taskDTO, userId);
        }

        public async Task<TaskModel?> GetTaskAsync(int taskId)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            return await _taskRepository.GetTaskAsync(currentUserId, taskId);
        }

        public async Task<TaskModel?> UpdateTaskAsync(int taskId, UpdateTaskDTO task)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            return await _taskRepository.UpdateTaskAsync(taskId, task, currentUserId);
        }

        public async Task<string> DeleteTaskAsync(int taskId)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            return await _taskRepository.DeleteTaskAsync(taskId, currentUserId);
        }
    }
}
