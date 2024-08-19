using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Interfaces;


namespace WebApi.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskModel>> GetTasksAsync(int userId)
        {
            try
            {
                return await _context.Tasks
                            .Where(task => task.UserId == userId)
                            .ToListAsync();
            }
            catch (Exception)
            {

                return [];
            }
        }
        public async Task<String> CreateTaskAsync(CreateTaskDTO taskDTO, int UserId)
        {
            try
            {
                User user = await _context.Users.FindAsync(UserId) ?? throw new InvalidOperationException("User not found in database.");

                TaskModel task = new()
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    UserId = UserId,
                    User = user

                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return "Task created successfully.";
            }
            catch (DbUpdateException)
            {
                return "Error creating task. Database error.";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }

        public async Task<TaskModel?> GetTaskAsync(int userId, int taskId)
        {
            try
            {
                TaskModel task = await _context.Tasks.FindAsync(taskId)
                ?? throw new InvalidOperationException("Task not found in database.");

                if (task.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Task is not associated with the user.");
                }

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
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<TaskModel> UpdateTaskAsync(int taskId, UpdateTaskDTO task, int userId)
        {
            try
            {
                TaskModel taskModel = _context.Tasks.Find(taskId)
                    ?? throw new InvalidOperationException("Task not found in database.");

                if (taskModel.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Task is not associated with the user.");
                }

                taskModel.Title = task.Title;
                taskModel.Description = task.Description;

                _context.Tasks.Update(taskModel);
                await _context.SaveChangesAsync();

                return taskModel;
            }
            catch (System.Exception ex)
            {
                // Aquí solo capturas excepciones genéricas que no han sido manejadas previamente
                throw new Exception("An error occurred while updating the task.", ex);
            }
        }




        public async Task<string> DeleteTaskAsync(int taskid, int userId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskid);
                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task), "Task not found in database.");
                }
                if (task.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Task is not associated with the user.");
                }
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return "Task deleted successfully.";
            }
            catch (ArgumentNullException)
            {
                return "Task not found in database";
            }
            catch (UnauthorizedAccessException)
            {
                return "Task is not associated with the user.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while deleting the task: {ex.Message}";
            }
        }






    }
}