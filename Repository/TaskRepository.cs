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
            catch (Exception ex)
            {
                // Loggear el error si es necesario
                throw new Exception("An error occurred while retrieving tasks.", ex);
            }
        }

        public async Task<string> CreateTaskAsync(CreateTaskDTO taskDTO, int userId)
        {
            try
            {
                User user = await _context.Users.FindAsync(userId)
                            ?? throw new InvalidOperationException("User not found in database.");

                TaskModel task = new()
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    UserId = userId,
                    User = user
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return "Task created successfully.";
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database error while creating task.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while creating task.", ex);
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the task.", ex);
            }
        }

        public async Task<TaskModel> UpdateTaskAsync(int taskId, UpdateTaskDTO task, int userId)
        {
            try
            {
                TaskModel taskModel = await _context.Tasks.FindAsync(taskId)
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the task.", ex);
            }
        }

        public async Task<string> DeleteTaskAsync(int taskId, int userId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
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
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the task.", ex);
            }
        }
    }
}
