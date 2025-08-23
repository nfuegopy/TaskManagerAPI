using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Shared.DTOs;
namespace TaskManagerAPI.Features.Users

{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

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

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto userDto)
        {
            var newUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

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