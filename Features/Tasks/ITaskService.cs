namespace TaskManagerAPI.Features.Tasks
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId);
        Task<TaskDto?> GetTaskByIdAndUserIdAsync(int id, int userId);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto, int userId);
        Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto taskDto, int userId);
        Task<bool> DeleteTaskAsync(int id, int userId);
        Task<IEnumerable<TaskDto>> CreateBulkTasksAsync(IEnumerable<CreateTaskDto> tasksDto, int userId);

        Task<IEnumerable<TaskDto>> GetFilteredTasksAsync(int userId, Status? status, Priority? priority);
    }
}