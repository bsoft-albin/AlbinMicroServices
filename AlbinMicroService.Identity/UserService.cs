using AlbinMicroService.Core.Utilities;
using AlbinMicroService.DataMappers.Dapper;

namespace AlbinMicroService.Identity
{
    public interface IUserService
    {
        Task<UserDto?> ValidateCredentialsAsync(string username, string password);
    }

    public class UserService(IDynamicMeths dynamicMeths, IDapperHelper dapperHelper) : IUserService
    {
        public async Task<UserDto?> ValidateCredentialsAsync(string username, string password)
        {
            string Query = @"select u.Id,u.Username,u.Password,ur.Role from users u
            INNER JOIN user_roles ur ON u.Id = ur.UserId
            where Username = @username AND Password = @password AND IsDeleted = 0 AND IsActive = 1 AND IsVerified = 1;";

            try
            {
                var user = await dapperHelper.QuerySingleOrDefaultAsync<UserDto>(Query, new { username, password });
                if (user != null)
                {
                    bool IsVerified = dynamicMeths.VerifyHash(password, user.Password);
                    if (IsVerified)
                    {
                        return user;
                    }
                    else {
                        return null;
                    }
                }
            }
            catch (Exception ex) // here Need to Integrate Serolog...
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}
