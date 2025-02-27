using AlbinMicroService.Core;
using AlbinMicroService.Users.Domain.DTOs;

namespace AlbinMicroService.Users.Application.Contracts
{
    public interface IUsersAppContract
    {
        Task<ApiBaseResponse> CreateUserAppAsync(UserDto userDto);
    }
}
