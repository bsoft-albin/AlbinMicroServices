﻿namespace AlbinMicroService.DataMappers.Dapper
{
    public interface IDapperHelper
    {
        MySqlConnection GetCreatedConnection();
        Task<int> ExecuteAsync(string sql, object? parameters = null);
        Task<T?> ExecuteScalarAsync<T>(string sql, object? parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
        Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null);
        Task<T> QueryFirstAsync<T>(string sql, object? parameters = null);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null);
        Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync2DataSets<T1, T2>(string sql, object? parameters = null);
        Task QueryMultipleAsyncReaderFunction(string sql, object? parameters, Func<SqlMapper.GridReader, Task> readFunc);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object? parameters);
    }
}
