using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Features.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var token = await _authService.LoginAsync(model);
            if (token == null)
            {
                return Unauthorized(new { message = "Email o contraseña inválidos." });
            }
            return Ok(new { token });
        }
    }
}