namespace TaskManagerAPI.Features.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        // La contraseña es opcional al actualizar
        public string? Password { get; set; }
    }
}