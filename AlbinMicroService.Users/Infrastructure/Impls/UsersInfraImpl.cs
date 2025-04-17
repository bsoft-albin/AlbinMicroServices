using AlbinMicroService.DataMappers.Dapper;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Infrastructure.Contracts;

namespace AlbinMicroService.Users.Infrastructure.Impls
{
    public class UsersInfraImpl(IDapperHelper _dapper, ILogger<UsersInfraImpl> _logger) : IUsersInfraContract
    {
        public async Task<short> CheckUsernameExistsOrNotInfraAsync(string username)
        {
            short count = 0;
			try
			{
                count = await _dapper.ExecuteScalarAsync<short>(UsersSqlQueries.UsernameExistCount, new { username });
			}
			catch (Exception ex)
			{
                _logger.LogError(ex, $"Error Occurred While Checking Users Count for {username}", username);
            }

            return count;
        }
    }
}
