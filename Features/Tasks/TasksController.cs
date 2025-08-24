using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Shared.DTOs;

namespace TaskManagerAPI.Features.Tasks
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserIdFromToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var userId = GetUserIdFromToken();
            var tasks = await _taskService.GetTasksByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var userId = GetUserIdFromToken();
            var task = await _taskService.GetTaskByIdAndUserIdAsync(id, userId);
            if (task == null)
            {
                return NotFound(new ApiResponseDto { Message = "Tarea no encontrada o no pertenece al usuario." });
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto taskDto)
        {
            var userId = GetUserIdFromToken();
            var newTask = await _taskService.CreateTaskAsync(taskDto, userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto taskDto)
        {
            var userId = GetUserIdFromToken();
            var updatedTask = await _taskService.UpdateTaskAsync(id, taskDto, userId);
            if (updatedTask == null)
            {
                return NotFound(new ApiResponseDto { Message = "Tarea no encontrada o no pertenece al usuario." });
            }
            return Ok(new ApiResponseDto { Message = $"Tarea '{updatedTask.Title}' actualizada correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserIdFromToken();
            var result = await _taskService.DeleteTaskAsync(id, userId);
            if (!result)
            {
                return NotFound(new ApiResponseDto { Message = "Tarea no encontrada o no pertenece al usuario." });
            }
            return Ok(new ApiResponseDto { Message = $"Tarea con ID {id} eliminada correctamente." });
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulkTasks([FromBody] IEnumerable<CreateTaskDto> tasksDto)
        {
            var userId = GetUserIdFromToken();
            var newTasks = await _taskService.CreateBulkTasksAsync(tasksDto, userId);


            return CreatedAtAction(nameof(GetAllTasks), newTasks);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredTasks([FromQuery] Status? status, [FromQuery] Priority? priority)
        {
            var userId = GetUserIdFromToken();
            var tasks = await _taskService.GetFilteredTasksAsync(userId, status, priority);
            return Ok(tasks);
        }

    }
}