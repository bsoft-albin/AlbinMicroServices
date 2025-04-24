namespace AlbinMicroService.DataMappers.RawADO
{
    public interface IAdoHelper
    {
        Task<int> ExecuteNonQueryAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text);
        Task<object?> ExecuteScalarAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text);
        Task<List<T>> ExecuteReaderAsync<T>(string query, Func<DbDataReader, T> map, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text);
        Task<DataTable> ExecuteDataTableAsync(string query, IEnumerable<DbParameter>? parameters = null, CommandType commandType = CommandType.Text);
        Task<int> ExecuteStoredProcedureAsync(string procName, DbParameter[] parameters);
    }
}
