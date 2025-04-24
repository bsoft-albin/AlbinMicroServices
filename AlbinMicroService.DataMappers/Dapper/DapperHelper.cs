namespace AlbinMicroService.DataMappers.Dapper
{
    public class DapperHelper(string _connectionString) : IDapperHelper
    {
        private MySqlConnection CreateConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentNullException(nameof(_connectionString));
            }

            return new MySqlConnection(_connectionString);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            var result = await connection.ExecuteScalarAsync<T>(sql, parameters) ?? throw new Exception("No Scalar Value Returned");
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<T>(sql, parameters);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstAsync<T>(sql, parameters);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync2DataSets<T1, T2>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sql, parameters);
            var result1 = await multi.ReadAsync<T1>();
            var result2 = await multi.ReadAsync<T2>();
            return (result1, result2);
        }

        public async Task QueryMultipleAsyncReaderFunction(string sql, object? parameters, Func<SqlMapper.GridReader, Task> readFunc)
        {
            using var connection = CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sql, parameters);
            await readFunc(multi);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object? parameters)
        {
            using var connection = CreateConnection();
            return await connection.QueryMultipleAsync(sql, parameters);
        }
    }
}
