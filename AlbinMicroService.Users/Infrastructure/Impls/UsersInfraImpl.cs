using AlbinMicroService.DataMappers.Dapper;
using AlbinMicroService.Libraries.Common.QueryManager;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Infrastructure.Contracts;
using Dapper;
using MySql.Data.MySqlClient;

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

        public async Task<short> CheckEmailExistsOrNotInfraAsync(string email)
        {
            short count = 0;
            try
            {
                count = await dapper.ExecuteScalarAsync<short>(UsersSqlQueries.EmailExistCount, new { email });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Occurred While Checking Email Count for @email");
            }

            return count;
        }

        public async Task<int> CreateUserAsync(UserRegisterDto userDto)
        {
            int InsertedRecord = 0;

            using MySqlConnection connection = dapper.GetCreatedConnection();
            await connection.OpenAsync();

            using MySqlTransaction transaction = await connection.BeginTransactionAsync();

            try
            {
                string UserRegisterQuery = SqlQueryCache.GetQuery("UserRegisterQuery");
                string UserRoleAdd = SqlQueryCache.GetQuery("UserRoleAdd");

                InsertedRecord = await connection.ExecuteScalarAsync<int>(UserRegisterQuery, new { username = userDto.Username, password = userDto.Password, email = userDto.Email }, transaction);
                if (InsertedRecord > 0)
                {
                    await connection.ExecuteAsync(UserRoleAdd, new { userId = InsertedRecord, role = userDto.Role}, transaction );
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "Error occurred while registering the user. Transaction rolled back.");
            }

            return InsertedRecord;
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
