using AlbinMicroService.Core.Repository;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Infrastructure.Contracts;

namespace AlbinMicroService.Users.Infrastructure.Impls
{
    public class UsersInfraImpl(ISqlServerMapper _sqlServer) : IUsersInfraContract
    {
        public async Task<short> CheckUsernameExistsOrNotInfraAsync(string username)
        {
			try
			{
                return await _sqlServer.ExecuteScalarAsync<short>(UsersSqlQueries.UsernameExistCount, new { username });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message);
            }
        }
    }
}
