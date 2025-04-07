using AlbinMicroService.Users.Infrastructure.Contracts;

namespace AlbinMicroService.Users.Infrastructure.Impls
{
    public class UsersInfraImpl() : IUsersInfraContract
    {
        public async Task<short> CheckUsernameExistsOrNotInfraAsync(string username)
        {
			try
			{
                await Task.Delay(1000); // Simulating some async work
                return 0;
                //return await _mysql.ExecuteScalarAsync<short>(UsersSqlQueries.UsernameExistCount, new { username });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message);
            }
        }
    }
}
