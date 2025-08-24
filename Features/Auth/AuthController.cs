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

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="model">Datos del nuevo usuario (Nombre, Email, Contraseña).</param>
        /// <returns>Los datos del usuario recién creado.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Autentica a un usuario y devuelve un token JWT.
        /// </summary>
        /// <param name="model">Credenciales del usuario (Email, Contraseña).</param>
        /// <returns>Un token JWT si las credenciales son correctas.</returns>
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