using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace WebApi.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _jwt = new JWT(_configuration["Jwt:Key"]);
            _httpContextAccessor = httpContextAccessor;
        }

        public TaskModel CreateTask(CreateTaskDTO taskDTO)
        {
            // Implementación del método
            TaskModel task = new TaskModel
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                UserId = 1
            };
            return task;
        }
    }
}