using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Shared.DTOs;

namespace TaskManagerAPI.Features.Tasks
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // IMPORTANTE: Este atributo protege todos los endpoints de este controlador.
                // Se necesita un Bearer Token válido para usarlos.
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Este es un método privado, no un endpoint. Ayuda a obtener el ID del usuario
        // de forma segura desde el token JWT en cada petición.
        private int GetUserIdFromToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }

        /// <summary>
        /// Obtiene una lista paginada de todas las tareas del usuario autenticado.
        /// </summary>
        /// <param name="pageNumber">Número de la página a obtener (opcional).</param>
        /// <param name="pageSize">Tamaño de la página (opcional).</param>
        /// <returns>Una lista de tareas del usuario.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var userId = GetUserIdFromToken();
            var tasks = await _taskService.GetTasksByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(tasks);
        }

        /// <summary>
        /// Obtiene una tarea específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la tarea que se desea obtener.</param>
        /// <returns>Los detalles de la tarea si se encuentra y pertenece al usuario.</returns>
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

        /// <summary>
        /// Crea una nueva tarea para el usuario autenticado.
        /// </summary>
        /// <param name="taskDto">Los datos de la nueva tarea a crear.</param>
        /// <returns>La tarea recién creada.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto taskDto)
        {
            var userId = GetUserIdFromToken();
            var newTask = await _taskService.CreateTaskAsync(taskDto, userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="id">El ID de la tarea a actualizar.</param>
        /// <param name="taskDto">Los nuevos datos para la tarea.</param>
        /// <returns>Un mensaje de confirmación si la actualización fue exitosa.</returns>
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

        /// <summary>
        /// Elimina una tarea específica.
        /// </summary>
        /// <param name="id">El ID de la tarea a eliminar.</param>
        /// <returns>Un mensaje de confirmación si la eliminación fue exitosa.</returns>
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

        /// <summary>
        /// Crea múltiples tareas en una sola petición.
        /// </summary>
        /// <param name="tasksDto">Una lista con los datos de las nuevas tareas.</param>
        /// <returns>La lista de tareas recién creadas.</returns>
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulkTasks([FromBody] IEnumerable<CreateTaskDto> tasksDto)
        {
            var userId = GetUserIdFromToken();
            var newTasks = await _taskService.CreateBulkTasksAsync(tasksDto, userId);
            return CreatedAtAction(nameof(GetAllTasks), newTasks);
        }

        /// <summary>
        /// Filtra las tareas del usuario por estado y/o prioridad.
        /// </summary>
        /// <param name="status">El estado por el cual filtrar (0=Pending, 1=InProgress, 2=Completed).</param>
        /// <param name="priority">La prioridad por la cual filtrar (0=Low, 1=Medium, 2=High).</param>
        /// <returns>Una lista de tareas que coinciden con los criterios de filtro.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetFilteredTasks([FromQuery] Status? status, [FromQuery] Priority? priority)
        {
            var userId = GetUserIdFromToken();
            var tasks = await _taskService.GetFilteredTasksAsync(userId, status, priority);
            return Ok(tasks);
        }
    }
}