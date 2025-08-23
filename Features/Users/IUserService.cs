namespace TaskManagerAPI.Features.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto);

        Task<bool> DeleteUserAsync(int id);
    }
}