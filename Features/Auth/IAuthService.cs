using TaskManagerAPI.Features.Users;

namespace TaskManagerAPI.Features.Auth
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterRequest model);
        Task<string?> LoginAsync(LoginRequest model);
    }
}