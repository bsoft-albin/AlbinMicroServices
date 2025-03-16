using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace AlbinMicroService.Core.Repository
{
    public interface IMySqlMapper
    {
        IDbConnection Connection { get; }

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);  // return the Single row Data table values

        Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text); // return the Data table with singel row oblject  with SP 
        Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);  // return the object values

        Task ExecuteAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text); // Insert, Update and Delete

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text);   // return the data table with more the one rows

        Task<GridReader> QueryMultipleAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text);  // return the Data Set values

        Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, CommandType commandType, object? parameters = null);

        void ExecuteScript(string script);

        object ExecuteScalar(string script);
    }

    public class MySqlMapper(string connectionString) : IMySqlMapper
    {
        private readonly string _connectionString = connectionString;

        public IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    return await Connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    return await Connection.QueryAsync<T>(sql, parameters, commandType: commandType, commandTimeout: 600);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    return await Connection.QuerySingleAsync<T>(sql, parameters, commandType: commandType);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    return await Connection.ExecuteScalarAsync<T>(sql, parameters, commandType: commandType);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task ExecuteAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    await Connection.ExecuteAsync(sql, parameters, commandType: commandType);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (Connection)
                {
                    return await Connection.QueryMultipleAsync(sql, parameters, commandType: commandType, commandTimeout: 180);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, CommandType commandType, object? parameters = null)
        {
            try
            {
                return await connection.ExecuteReaderAsync(sql, parameters, commandType: commandType, commandTimeout: 180);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ExecuteScript(string script)
        {
            try
            {
                using (Connection)
                {
                    Connection.Open();
                    Connection.Execute(script);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public object ExecuteScalar(string script)
        {
            object? result = null;
            try
            {
                // code to execute script file in dapper
                using (Connection)
                {
                    Connection.Open();
                    result = Connection.ExecuteScalar(script);
                }

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
