using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Interfaces
{
    public interface ITaskService
    {
        public Task<List<TaskModel>> GetTasksAsync();
        public Task<string> CreateTaskAsync(CreateTaskDTO taskDTO);
        public Task<TaskModel?> GetTaskAsync(int taskId);
        public Task<TaskModel?> UpdateTaskAsync(int taskId, UpdateTaskDTO task);
        public Task<string> DeleteTaskAsync(int taskId);

    }
}