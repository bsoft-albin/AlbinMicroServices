namespace AlbinMicroService.Users.Infrastructure.Contracts
{
    public interface IUsersInfraContract
    {
        Task<short> CheckUsernameExistsOrNotInfraAsync(string username);
        Task<string?> GetUserRoleInfraAsync(string username);
        Task<short> CheckEmailExistsOrNotInfraAsync(string email);
        Task<int> CreateUserAsync(UserRegisterDto userDto);
    }
}
