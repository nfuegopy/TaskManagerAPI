namespace TaskManagerAPI.Features.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int? pageNumber, int? pageSize);

        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto);

        Task<bool> DeleteUserAsync(int id);
    }
}