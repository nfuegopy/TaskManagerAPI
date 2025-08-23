using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Features.Users;

namespace TaskManagerAPI.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IUserService userService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userService = userService;
            _configuration = configuration;
        }

        public Task<UserDto> RegisterAsync(RegisterRequest model)
        {
            var userDto = new CreateUserDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };
            // Delega la creación al UserService que ya funciona
            return _userService.CreateUserAsync(userDto);
        }

        public async Task<string?> LoginAsync(LoginRequest model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return null; // Credenciales incorrectas
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
 //               Expires = DateTime.UtcNow.AddHours(8),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}