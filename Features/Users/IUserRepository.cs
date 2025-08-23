namespace TaskManagerAPI.Features.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
    }
}