using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Utils;
using Microsoft.EntityFrameworkCore;



namespace WebApi.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Cookies _cookies;

        public TaskService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null");
            _jwt = new JWT(jwtKey);
            _httpContextAccessor = httpContextAccessor;
            _cookies = new Cookies();
        }

        public async Task<String> CreateTask(CreateTaskDTO taskDTO)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
                string cookie = _cookies.GetCookie("token", httpContext.Request);

                string email = _jwt.DecodeToken(cookie);

                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("User not found.");
                }

                int UserId = user.Id;

                TaskModel task = new()
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    UserId = UserId,
                    User = await _context.Users.FindAsync(UserId) ?? throw new InvalidOperationException("User not found in database.")
                };

                // Guardar la tarea en la base de datos
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return "Task created successfully.";
            }
            catch (System.Exception)
            {
                throw;
            }
        }



        public async Task<List<TaskModel>> GetTaskAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            string cookie = _cookies.GetCookie("token", httpContext.Request);
            if (string.IsNullOrEmpty(cookie))
            {
                throw new UnauthorizedAccessException("Token is missing.");
            }

            string email = _jwt.DecodeToken(cookie);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new UnauthorizedAccessException("User not found.");
            int userId = user.Id;
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<TaskModel> GetTaskAsync(string Id)
        {
            int id = int.Parse(Id);
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("httpContext is null");
            string cookie = _cookies.GetCookie("token", httpContext.Request);
            if (string.IsNullOrEmpty(cookie))
            {
                throw new UnauthorizedAccessException("Token is missing.");
            }
            string email = _jwt.DecodeToken(cookie);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new UnauthorizedAccessException("User not found.");

            var task = await _context.Tasks.FindAsync(id) ?? throw new InvalidOperationException("Task not found.");

            if (task.UserId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this task.");
            }
            return task;
        }

























    }
}