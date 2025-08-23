namespace TaskManagerAPI.Features.Tasks
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Task>> GetTasksByUserIdAsync(int userId);
        Task<Task?> GetTaskByIdAsync(int id);
        Task<Task> CreateTaskAsync(Task task);
        Task<Task> UpdateTaskAsync(Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<Task>> CreateBulkTasksAsync(IEnumerable<Task> tasks);

        Task<IEnumerable<Task>> GetFilteredTasksAsync(int userId, Status? status, Priority? priority);

    }
}