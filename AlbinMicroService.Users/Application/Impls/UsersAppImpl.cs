using AlbinMicroService.Core;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Infrastructure.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace AlbinMicroService.Users.Application.Impls
{
    public class UsersAppImpl(IUsersDomainContract usersDomain, IUsersInfraContract usersInfraContract) : IUsersAppContract
    {
        public async Task<ApiBaseResponse> CreateUserAppAsync(UserRegisterDto userDto)
        {
            ApiBaseResponse apiBaseResponse = new();

            if (userDto != null)
            {
                ValidatorTemplate validObj = usersDomain.ValidateUserDto(userDto);

                if (validObj != null && validObj.IsValidated)
                {
                    // check whether username exists
                    bool isUsernameExists = await usersDomain.VerifyUsernameExistsOrNotAsync(userDto.Username);

                    // check whether email exists
                    bool isEmailExists = await usersDomain.VerifyEmailExistsOrNotAsync(userDto.Email);

                    if (!isUsernameExists && !isEmailExists)
                    {
                        userDto.Password = usersDomain.HashUserPassword(userDto.Password);

                        if (!string.IsNullOrEmpty(userDto.Password))
                        {
                            // call Db to save user
                            int CreatedId = await usersInfraContract.CreateUserAsync(userDto);
                            //await usersDomain.SendWelcomeEmailToUserAsync(userDto.Email, userDto.Username);

                            if (CreatedId > 0)
                            {
                                apiBaseResponse.StatusCode = HttpStatusCodes.Status201Created;
                                apiBaseResponse.StatusMessage = HttpStatusMessages.Status201Created;
                            }
                            else
                            {
                                apiBaseResponse.StatusCode = HttpStatusCodes.Status500InternalServerError;
                                apiBaseResponse.StatusMessage = HttpStatusMessages.Status500InternalServerError;
                            }
                        }
                        else
                        {
                            apiBaseResponse.StatusCode = HttpStatusCodes.Status500InternalServerError;
                            apiBaseResponse.StatusMessage = HttpStatusMessages.Status500InternalServerError;
                        }
                    }
                    else if(isUsernameExists)
                    {
                        apiBaseResponse.StatusCode = HttpStatusCodes.Status409Conflict;
                        apiBaseResponse.StatusMessage = CustomHttpStatusMessages.UsernameExists;
                    }
                    else if (isEmailExists)
                    {
                        apiBaseResponse.StatusCode = HttpStatusCodes.Status409Conflict;
                        apiBaseResponse.StatusMessage = CustomHttpStatusMessages.EmailExists;
                    }
                    else
                    {
                        apiBaseResponse.StatusCode = CustomHttpStatusCodes.UnXpectedError;
                        apiBaseResponse.StatusMessage = CustomHttpStatusMessages.UnXpectedError;
                    }
                }
                else
                {
                    return new ValidationErrorResponse<List<string>>()
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

        public async Task<string?> GetUserRoleAppAsync(string username)
        {
            string? Role = await usersInfraContract.GetUserRoleInfraAsync(username);

            return Role.IsNullOrEmpty() ? null : Role;
        }
    }
}
