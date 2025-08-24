using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Shared.DTOs;

namespace TaskManagerAPI.Features.Users
{
    [ApiController]
    [Route("api/[controller]")]
    // NOTA: En una aplicación real, este controlador completo estaría protegido con [Authorize],
    // posiblemente con roles de administrador: [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtiene una lista paginada de todos los usuarios del sistema.
        /// </summary>
        /// <remarks>
        /// Endpoint de gestión. En un escenario real, solo un administrador tendría acceso a esta lista.
        /// </remarks>
        /// <param name="pageNumber">Número de la página a obtener (opcional).</param>
        /// <param name="pageSize">Tamaño de la página (opcional).</param>
        /// <returns>Una lista de usuarios.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario específico por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a obtener.</param>
        /// <returns>Los detalles del usuario si se encuentra.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema (Ruta de Gestión).
        /// </summary>
        /// <remarks>
        /// **Propósito:** Este es el endpoint de "administración". A diferencia de `/api/auth/register` (que es público),
        /// esta ruta sería utilizada por un administrador del sistema para crear cuentas directamente.
        /// Por ejemplo, para dar de alta a un nuevo empleado.
        /// </remarks>
        /// <param name="userDto">Los datos del nuevo usuario a crear.</param>
        /// <returns>El usuario recién creado.</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto userDto)
        {
            var newUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="id">El ID del usuario a actualizar.</param>
        /// <param name="userDto">Los nuevos datos para el usuario.</param>
        /// <returns>Un mensaje de confirmación.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto userDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            if (updatedUser == null)
            {
                return NotFound(new ApiResponseDto { Message = $"Usuario con ID {id} no encontrado." });
            }
            return Ok(new ApiResponseDto { Message = $"Usuario '{updatedUser.Name}' actualizado correctamente." });
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        /// <returns>Un mensaje de confirmación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponseDto { Message = $"Usuario con ID {id} no encontrado." });
            }
            return Ok(new ApiResponseDto { Message = $"Usuario con ID {id} eliminado correctamente." });
        }
    }
}