namespace TaskManagerAPI.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {

            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception($"El email '{userDto.Email}' ya está registrado.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            return new UserDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null; // Devuelve null si no se encuentra

            user.Name = userDto.Name;
            user.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            // Devuelve el DTO del usuario actualizado
            return new UserDto
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                CreatedAt = updatedUser.CreatedAt
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            // Convertimos la lista de User a una lista de UserDto
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
    }
}