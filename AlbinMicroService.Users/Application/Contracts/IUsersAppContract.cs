using AlbinMicroService.Core;
using AlbinMicroService.Users.Domain.Models.Dtos;

namespace AlbinMicroService.Users.Application.Contracts
{
    public interface IUsersAppContract
    {
        Task<ApiBaseResponse> CreateUserAppAsync(UserDto userDto);
    }
}
