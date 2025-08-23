using TaskManagerAPI.Features.Users; 

namespace TaskManagerAPI.Features.Tasks
{
    public enum Priority { Low, Medium, High }
    public enum Status { Pending, InProgress, Completed }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}