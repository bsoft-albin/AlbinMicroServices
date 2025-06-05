using AlbinMicroService.DataMappers.Dapper;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Infrastructure.Contracts;

namespace AlbinMicroService.Users.Infrastructure.Impls
{
    public class UsersInfraImpl(IDapperHelper dapper, ILogger<UsersInfraImpl> logger) : IUsersInfraContract
    {
        public async Task<short> CheckUsernameExistsOrNotInfraAsync(string username)
        {
            short count = 0;
            try
            {
                count = await dapper.ExecuteScalarAsync<short>(UsersSqlQueries.UsernameExistCount, new { username });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Occurred While Checking Users Count for @username");
            }

            return count;
        }

        public async Task<string?> GetUserRoleInfraAsync(string username)
        {
            string? Role = null;
            try
            {
                Role = await dapper.ExecuteScalarAsync<string>(UsersSqlQueries.UserRoleGet, new { username });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Occurred While Getting the Role for @username");
            }

            return Role;
        }
    }
}
