namespace AlbinMicroService.DataMappers.RawADO
{
    public class AdoHelper(string providerInvariantName, string connectionString) : IAdoHelper
    {
        private readonly string _connectionString = connectionString;
        private readonly DbProviderFactory _factory = DbProviderFactories.GetFactory(providerInvariantName);

        private DbConnection CreateConnection()
        {
            var conn = _factory.CreateConnection();
            conn!.ConnectionString = _connectionString;
            return conn;
        }

        private DbCommand CreateCommand(string query, DbConnection conn, IEnumerable<DbParameter>? parameters, CommandType type)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = type;
            if (parameters != null)
            {
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);
            }
            return cmd;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();
            await using var cmd = CreateCommand(query, conn, parameters, commandType);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();
            await using var cmd = CreateCommand(query, conn, parameters, commandType);
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<List<T>> ExecuteReaderAsync<T>(string query, Func<DbDataReader, T> map, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text)
        {
            var result = new List<T>();
            await using var conn = CreateConnection();
            await conn.OpenAsync();
            await using var cmd = CreateCommand(query, conn, parameters, commandType);
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
            while (await reader.ReadAsync())
            {
                result.Add(map(reader));
            }
            return result;
        }

        public async Task<DataTable> ExecuteDataTableAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();
            await using var cmd = CreateCommand(query, conn, parameters, commandType);
            var adapter = _factory.CreateDataAdapter()!;
            adapter.SelectCommand = cmd;
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procName, DbParameter[] parameters)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();
            await using var cmd = CreateCommand(procName, conn, parameters, CommandType.StoredProcedure);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
