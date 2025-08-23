using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Features.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }


        public async Task<bool> DeleteTaskAsync(int id)
        {
            var taskToDelete = await _context.Tasks.FindAsync(id);
            if (taskToDelete == null)
            {
                return false;
            }

            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Task?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<Task>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Task> UpdateTaskAsync(Task task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return task;
        }


        public async Task<IEnumerable<Task>> CreateBulkTasksAsync(IEnumerable<Task> tasks)
        {
           
            await _context.Tasks.AddRangeAsync(tasks);
            await _context.SaveChangesAsync();
            return tasks;
        }

        public async Task<IEnumerable<Task>> GetFilteredTasksAsync(int userId, Status? status, Priority? priority)
        {
            var query = _context.Tasks.Where(t => t.UserId == userId);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            return await query.ToListAsync();
        }
    }
}