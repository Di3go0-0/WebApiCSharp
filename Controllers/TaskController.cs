// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Linq;
// using WebApi.Services;
// using WebApi.Models;

// namespace WebApi.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class TaskController : ControllerBase
//     {
//         private readonly TaskService _taskService;

//         public TaskController(TaskService taskService)
//         {
//             _taskService = taskService;
//         }

//         [HttpPost]
//         public IActionResult CreateTask([FromBody] JObject data)
//         {
//             string title = data["title"]?.ToString();
//             string description = data["description"]?.ToString();
//             if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
//             {
//                 return BadRequest("Title and description are required.");
//             }

//             var task = new WebApi.Models.Task
//             {
//                 Title = title,
//                 Description = description
//             };

//             var result = _taskService.CreateTask(task);
//             if (result == "Task created successfully.")
//             {
//                 return Ok(result);
//             }
//             return BadRequest(result);
//         }
//     }
// }