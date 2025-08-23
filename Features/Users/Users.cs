
using Task = TaskManagerAPI.Features.Tasks.Task;
namespace TaskManagerAPI.Features.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();

    }
}
