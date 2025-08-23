using Microsoft.EntityFrameworkCore;
using Task = TaskManagerAPI.Features.Tasks.Task;
using TaskManagerAPI.Features.Users;

namespace TaskManagerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aquí definimos la relación entre User y Task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.User) // Una Tarea tiene un Usuario
                .WithMany(u => u.Tasks) // Un Usuario tiene muchas Tareas
                .HasForeignKey(t => t.UserId); // La clave foránea es UserId
        }
    }
}