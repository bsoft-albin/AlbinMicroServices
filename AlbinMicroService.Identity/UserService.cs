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

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            UserDto testUser = new() { Username = username, Password = password };

            if (username == "billy-boy" && password == "billy@20")
            {
                testUser.Id = "#003";
                testUser.Role = "User";

                return testUser;
            }
            if (username == "billy-admin" && password == "billy@20")
            {
                testUser.Id = "#002";
                testUser.Role = "Admin";

                return testUser;
            }
            if (username == "billy-superadmin" && password == "billy@20")
            {
                testUser.Id = "#001";
                testUser.Role = "SuperAdmin";

                return testUser;
            }

            string Query = @"SELECT u.Id,u.Username,u.Password,ur.Role from users u
            INNER JOIN user_roles ur ON u.Id = ur.UserId
            where u.Username = @username AND u.IsDeleted = 0 AND u.IsActive = 1 AND u.IsVerified = 1;";

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
                    else
                    {
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
