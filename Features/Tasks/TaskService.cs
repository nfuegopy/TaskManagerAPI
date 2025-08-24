using TaskManagerAPI.Features.Users; // Necesario para la conversión a DTO

namespace TaskManagerAPI.Features.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto, int userId)
        {
            var task = new Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                Status = taskDto.Status,
                UserId = userId // Asignamos el ID del usuario del token
            };

            var createdTask = await _taskRepository.CreateTaskAsync(task);

            return new TaskDto
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                DueDate = createdTask.DueDate,
                Priority = createdTask.Priority,
                Status = createdTask.Status,
                CreatedAt = createdTask.CreatedAt,
                UserId = createdTask.UserId
            };
        }

        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                // No se encontró la tarea o no pertenece al usuario
                return false;
            }
            return await _taskRepository.DeleteTaskAsync(id);
        }

        public async Task<TaskDto?> GetTaskByIdAndUserIdAsync(int id, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return null;
            }

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            };
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId, int? pageNumber, int? pageSize)

        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId, pageNumber, pageSize);

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            });
        }


        public async Task<IEnumerable<TaskDto>> CreateBulkTasksAsync(IEnumerable<CreateTaskDto> tasksDto, int userId)
        {
            var tasks = tasksDto.Select(dto => new Task
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                Status = dto.Status,
                UserId = userId 
            }).ToList();

            var createdTasks = await _taskRepository.CreateBulkTasksAsync(tasks);

            return createdTasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            });
        }


        public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto taskDto, int userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return null;
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Priority = taskDto.Priority;
            task.Status = taskDto.Status;

            var updatedTask = await _taskRepository.UpdateTaskAsync(task);

            return new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                DueDate = updatedTask.DueDate,
                Priority = updatedTask.Priority,
                Status = updatedTask.Status,
                CreatedAt = updatedTask.CreatedAt,
                UserId = updatedTask.UserId
            };
        }

        public async Task<IEnumerable<TaskDto>> GetFilteredTasksAsync(int userId, Status? status, Priority? priority)
        {
            var tasks = await _taskRepository.GetFilteredTasksAsync(userId, status, priority);
            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId
            });
        }
    }
}