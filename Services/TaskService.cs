using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Interfaces;




namespace WebApi.Services
{
    public class TaskService
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ITaskRepository _taskRepository;

        public TaskService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITaskRepository taskRepository, AuthenticationService authenticationService)
        {
            
            _taskRepository = taskRepository;
            _authenticationService = authenticationService;

        }
        public async Task<List<TaskModel>> GetTasksAsync()
        {
            int userId = _authenticationService.AuthenticateUser();
            List<TaskModel> list = await _taskRepository.GetTasksAsync(userId);
            return list;
        }

        public async Task<String> CreateTask(CreateTaskDTO taskDTO)
        {
            try
            {
                int userId = _authenticationService.AuthenticateUser();
                String response = await _taskRepository.CreateTaskAsync(taskDTO, userId);

                return response;
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }



        public async Task<TaskModel?> GetTaskAsync(int taskId)
        {
            try
            {
                int currentUserId = _authenticationService.AuthenticateUser();
                TaskModel? task = await _taskRepository.GetTaskAsync(currentUserId, taskId);
                return task;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<TaskModel?> UpdateTaskAsync(int taskId, UpdateTaskDTO task)
        {
            int currentUserId = _authenticationService.AuthenticateUser();
            TaskModel taskModel = await _taskRepository.UpdateTaskAsync(taskId, task, currentUserId);
            return taskModel;
        }


        public async Task<string> DeleteTaskAsync(int taskId)
        {
            try
            {
                int currentUserId = _authenticationService.AuthenticateUser();
                string response = await _taskRepository.DeleteTaskAsync(taskId, currentUserId);
                return response;
            }
            catch (ArgumentNullException)
            {
                return "Task not found in database.";
            }
            catch (UnauthorizedAccessException)
            {
                return "Task is not associated with the user.";
            }
            catch (System.Exception)
            {
                return "An error occurred while deleting the task.";
            }

        }













    }
}