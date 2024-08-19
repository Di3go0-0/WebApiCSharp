using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

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
            catch (System.Exception)
            {

                throw new Exception("An error occurred while getting the tasks.");
            }
        }

        public async Task<TaskModel> GetTaskAsync(int userId, int taskId)
        {
            try
            {
                TaskModel task = await _context.Tasks.FindAsync(taskId) ?? throw new InvalidOperationException("Task not found in database.");
                if (task.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Task is not associated with the user.");
                }

                return task;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"An error occurred while getting the task: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception($"An error occurred while getting the task: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                throw new Exception($"An error occurred while getting the task: {ex.Message}");
            }
        }

        public async Task<TaskModel> CreateTaskAsync(CreateTaskDTO taskDTO, int UserId)
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
                return task;
            }
            catch (System.Exception)
            {
                throw new Exception("An error occurred while creating the task.");
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
            catch (ArgumentNullException ex)
            {
                throw new Exception($"An error occurred while deleting the task: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception($"An error occurred while deleting the task: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the task.", ex);
            }
        }





        public Task<TaskModel> UpdateTaskAsync(int taskId, UpdateTaskDTO task, int userId)
        {
            try
            {
                TaskModel taskModel = _context.Tasks.Find(taskId) ?? throw new InvalidOperationException("Task not found in database.");
                if (taskModel.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Task is not associated with the user.");
                }
                taskModel.Title = task.Title;
                taskModel.Description = task.Description;

                _context.Tasks.Update(taskModel);
                _context.SaveChanges();
                return Task.FromResult(taskModel);
            }
            catch (System.Exception)
            {

                throw new Exception("An error occurred while updating the task.");
            }
        }
    }
}