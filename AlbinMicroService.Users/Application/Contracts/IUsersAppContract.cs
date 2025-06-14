using AlbinMicroService.Core;

namespace AlbinMicroService.Users.Application.Contracts
{
    public interface IUsersAppContract
    {
        Task<ApiBaseResponse> CreateUserAppAsync(UserRegisterDto userDto);
        Task<string?> GetUserRoleAppAsync(string username);
    }
}
