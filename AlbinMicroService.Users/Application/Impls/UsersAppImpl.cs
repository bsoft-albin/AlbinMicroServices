using AlbinMicroService.Core;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.DTOs;

namespace AlbinMicroService.Users.Application.Impls
{
    public class UsersAppImpl(IUsersDomainContract _usersDomain) : IUsersAppContract
    {
        public async Task<ApiBaseResponse> CreateUserAsync(UserDto userDto)
        {
            ApiBaseResponse apiBaseResponse = new();
            if (userDto != null)
            {
                ValidatorTemplate validObj = _usersDomain.ValidateUserDto(userDto);
                if (validObj != null && validObj.IsValidated)
                {
                    userDto.Password = _usersDomain.HashUserPassword(userDto.Password);
                }
            }
            else
            {
                apiBaseResponse.StatusCode = HttpStatusCodes.Status400BadRequest;
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status400BadRequest;
            }

            return apiBaseResponse;
        }
    }
}
