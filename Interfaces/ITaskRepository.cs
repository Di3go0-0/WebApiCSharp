using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Interfaces
{
    public interface ITaskRepository
    {
        public Task<List<TaskModel>> GetTasksAsync(int userId);
        public Task<TaskModel> GetTaskAsync(int taskId, int userId);
        public Task<TaskModel> CreateTaskAsync(CreateTaskDTO taskDTO, int UserId);
        public Task<TaskModel> UpdateTaskAsync(int taskId, UpdateTaskDTO task, int userId);
        public Task<string> DeleteTaskAsync(int taskId, int userId);
    }
}