namespace AlbinMicroService.Users.Domain.Models.Dtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserRegisterDto
    {
        public string Username { get; init; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; init; } = null!;
        public string Role { get; set; } = null!;
    }
}