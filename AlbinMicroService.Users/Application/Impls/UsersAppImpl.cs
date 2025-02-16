using AlbinMicroService.Core;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.DTOs;
using Microsoft.IdentityModel.Tokens;

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
                    if (!userDto.Password.IsNullOrEmpty())
                    {
                        // call Db to save user
                        bool dbResponse = true; // here repo call to save in db
                        bool mailResponse = await _usersDomain.SendWelcomeEmailToUser(userDto.Email, userDto.Username);
                        if (dbResponse && mailResponse)
                        {
                            apiBaseResponse.StatusCode = HttpStatusCodes.Status201Created;
                            apiBaseResponse.StatusMessage = HttpStatusMessages.Status201Created;
                        }
                        else
                        {
                            if (!dbResponse)
                            {
                                if (!mailResponse)
                                {

                                }
                            }
                            else {
                                apiBaseResponse.StatusCode = HttpStatusCodes.Status500InternalServerError;
                                apiBaseResponse.StatusMessage = HttpStatusMessages.Status500InternalServerError;
                            }
                        }
                    }
                    else
                    {
                        apiBaseResponse.StatusCode = HttpStatusCodes.Status500InternalServerError;
                        apiBaseResponse.StatusMessage = HttpStatusMessages.Status500InternalServerError;
                    }
                }
                else
                {
                    return new ApiErrorResponse<List<string>>()
                    {
                        StatusCode = HttpStatusCodes.Status400BadRequest,
                        StatusMessage = HttpStatusMessages.Status400BadRequest,
                        ErrorDetails = validObj?.Errors ?? new List<string>()
                    };
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
