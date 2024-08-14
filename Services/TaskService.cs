// using WebApi.Context;
// using WebApi.Models;
// using WebApi.Utils;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Configuration;

// namespace WebApi.Services
// {
//     public class TaskService
//     {
//         private readonly AppDbContext _context;
//         private readonly IConfiguration _configuration;
//         private readonly JWT _jwt;
//         private readonly IHttpContextAccessor _httpContextAccessor;

//         public TaskService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
//         {
//             _context = context;
//             _configuration = configuration;
//             _jwt = new JWT(_configuration["Jwt:Key"]);
//             _httpContextAccessor = httpContextAccessor;
//         }

//         public string CreateTask(WebApi.Models.Task task)
//         {
//             if (string.IsNullOrEmpty(task.Title) || string.IsNullOrEmpty(task.Description))
//             {
//                 return "Title and description are required.";
//             }

//             var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
//             if (string.IsNullOrEmpty(token))
//             {
//                 return "Token not found in cookies.";
//             }

//             string email = _jwt.DecodeToken(token);
//             var user = _context.Users.SingleOrDefault(u => u.Email == email);
//             if (user == null)
//             {
//                 return "User not found.";
//             }

//             task.UserId = user.Id;
//             _context.Tasks.Add(task);
//             _context.SaveChanges();

//             return "Task created successfully.";
//         }
//     }
// }