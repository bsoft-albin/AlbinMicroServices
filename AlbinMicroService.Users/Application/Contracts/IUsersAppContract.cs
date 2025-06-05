using AlbinMicroService.Core;

namespace AlbinMicroService.Users.Application.Contracts
{
    public interface IUsersAppContract
    {
        Task<ApiBaseResponse> CreateUserAppAsync(UserDto userDto);
        Task<string?> GetUserRoleAppAsync(string username);
    }
}
