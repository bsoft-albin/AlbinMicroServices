using AlbinMicroService.Core;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.DTOs;

namespace AlbinMicroService.Users.Application.Impls
{
    public class UsersAppImpl(IUsersDomainContract _usersDomain) : IUsersAppContract
    {
        public async Task<ApiBaseResponse> CreateUserAppAsync(UserDto userDto)
        {
            ApiBaseResponse apiBaseResponse = new();

            if (userDto != null)
            {
                ValidatorTemplate validObj = _usersDomain.ValidateUserDto(userDto);

                if (validObj != null && validObj.IsValidated)
                {
                    // check whether username exists
                    bool isUsernameExists = await _usersDomain.VerifyUsernameExistsOrNotAsync(userDto.Username);

                    if (!isUsernameExists)
                    {
                        userDto.Password = _usersDomain.HashUserPassword(userDto.Password);

                        if (!string.IsNullOrEmpty(userDto.Password))
                        {
                            // call Db to save user
                            bool dbResponse = true; // here repo call to save in db
                            bool mailResponse = await _usersDomain.SendWelcomeEmailToUserAsync(userDto.Email, userDto.Username);

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
                                else
                                {
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
                        apiBaseResponse.StatusCode = HttpStatusCodes.Status409Conflict;
                        apiBaseResponse.StatusMessage = CustomHttpStatusMessages.UsernameExists;
                    }
                }
                else
                {
                    return new ApiErrorResponse<List<string>>()
                    {
                        StatusCode = HttpStatusCodes.Status400BadRequest,
                        StatusMessage = HttpStatusMessages.Status400BadRequest,
                        ErrorDetails = validObj?.Errors ?? []
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
