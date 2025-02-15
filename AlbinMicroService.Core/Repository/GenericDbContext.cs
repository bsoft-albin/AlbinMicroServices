using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AlbinMicroService.Core.Repository
{
    public interface IDBRepository<T> where T : class
    {
        // Add new entity
        Task<T> AddAsync(T entity);

        Task<int> AddEntityAsync(T entity);

        Task<int> AddRangeAsync(List<T> entity);

        Task<T> FetchDefaultAsync(Expression<Func<T, bool>> predicate);

        // Get by Id
        Task<T> GetByIdAsync(long id);

        Task<T> GetByIdAsync(Guid id);

        // Remove entity
        Task<int> RemoveAsync(T entity);

        Task<int> RemoveEntityAsync(T entity);

        // Get all entities
        Task<IEnumerable<T>> GetAllAsync(bool include = false);

        // Get all needed entities with filter
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        // Get all needed entities with selected columns
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector);

        // Find entities based on a predicate (using Func<>)
        Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate);

        Task<T> FindAsync(Func<T, bool> predicate);

        // Update entity
        Task<int> UpdateAsync(T entity);

        Task<int> UpdateEntity(T entity);

        Task<int> UpdateRangeAsync(List<T> entity);

        // Delete entity
        Task<int> DeleteAsync(long id);

        Task<int> RemoveRangeAsync(List<T> entity);

        Task<int> DeleteAsync(Guid id);

        Task<IEnumerable<TDTO>> ExecuteSPAsync<TDTO>(string storedProcedure, object? parameters);

        Task<IEnumerable<TDTO>> ExecuteFunctionAsync<TDTO>(string functionName, object parameters);

        Task<IEnumerable<TDTO>> ExecuteQueryAsync<TDTO>(string sQuery, object parameters);
    }

    public class DBRepository<T> : IDBRepository<T> where T : class
    {
        private readonly DbContext _context;  // Use DbContext directly
        private readonly DbSet<T> _dbSet;

        public DBRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // Add new entity
        public async Task<T> AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //newly added
        public async Task<int> AddEntityAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            int result = 0;
            try
            {
                await _dbSet.AddAsync(entity);
                result = await _context.SaveChangesAsync(); // returns no of rows added to the Db.
                if (result > 0)
                {
                    return 201; //inserted succesfully
                }
                else
                {
                    return 422; //unprocessable entity.
                }
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public async Task<int> AddRangeAsync(List<T> entities)
        {
            // Check for null to avoid exceptions
            ArgumentNullException.ThrowIfNull(entities);

            // Use AddRange instead of AddRangeAsync
            _dbSet.AddRange(entities);

            // Save changes asynchronously
            return await _context.SaveChangesAsync();
        }

        public async Task<T> FetchDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        // Get by Id
        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync(bool include = false)
        {
            if (include)
            {
                return await _dbSet.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                // As No Tracking
                return await _dbSet.AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"> Predicate to filter results</param>
        /// <param name="selector">Selector to map the result </param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)  // Apply WHERE condition
                .Select(selector)   // Select the projection
                .ToListAsync();     // Execute query and return results
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .AsNoTracking()
                .AsNoTracking() // Improves performance by not tracking the entities
                .Select(selector)
                .ToListAsync();
        }

        // Find entities based on a predicate
        public async Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().Where(predicate).ToList());
        }

        // Find entities based on a predicate
        public async Task<T> FindAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().FirstOrDefault(predicate));
        }

        // Update entity
        public async Task<int> UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        //newly added
        public async Task<int> UpdateEntity(T entity)
        {
            int result = 0;
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                // Save changes to the database
                result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return 202; //updated succesfully
                }
                else
                {
                    return 422; //unprocessable entity.
                }
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public async Task<int> UpdateRangeAsync(List<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            _dbSet.UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        // Remove entity
        public async Task<int> RemoveEntityAsync(T entity)
        {
            int result = 0;
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                _dbSet.Remove(entity);
                result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return 205; //deleted succesfully
                }
                else
                {
                    return 422; //unprocessable entity.
                }
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(long id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> RemoveRangeAsync(List<T> entities)
        {
            // Check if the entities list is null or empty to avoid unnecessary database operations
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities), "The entities list cannot be null or empty.");
            }

            // Remove the entities from the DbSet
            _dbSet.RemoveRange(entities);

            // Save changes asynchronously and return the number of affected rows
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TDTO>> ExecuteSPAsync<TDTO>(string storedProcedure, object parameters = null)
        {
            var result = new List<TDTO>();

            using (var dbConnection = _context.Database.GetDbConnection())
            {
                dbConnection.Open();
                // Execute the stored procedure using Dapper
                var entities = await dbConnection.QueryAsync<TDTO>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                result = entities.AsList(); // Convert to List<TDTO>
            }
            return result;
        }

        public async Task<IEnumerable<TDTO>> ExecuteQueryAsync<TDTO>(string sQuery, object parameters = null)
        {
            var result = new List<TDTO>();
            using (var dbConnection = _context.Database.GetDbConnection())
            {
                await dbConnection.OpenAsync();
                // Execute the Query using Dapper
                var entities = await dbConnection.QueryAsync<TDTO>(sQuery, parameters, commandType: CommandType.Text);
                result = entities.AsList(); // Convert to List<TDTO>
            }
            return result;
        }

        internal record NewRecord(string Name, object Value);

        public async Task<IEnumerable<TDTO>> ExecuteFunctionAsync<TDTO>(string functionName, object parameters = null)
        {
            using var dbConnection = _context.Database.GetDbConnection();
            await dbConnection.OpenAsync();
            var result = new List<TDTO>();
            try
            {
                var query = new StringBuilder($"SELECT * FROM {functionName}(");
                var dapperParameters = new DynamicParameters();

                if (parameters != null)
                {
                    var parametersList = new List<string>();
                    var properties = parameters switch
                    {
                        Dictionary<string, object> dict => dict.Select(kvp => new NewRecord(kvp.Key, kvp.Value)),
                        _ => parameters.GetType().GetProperties().Select(p => new NewRecord(p.Name, p.GetValue(parameters)))
                    };

                    foreach (var prop in properties)
                    {
                        string paramName = $"@{prop.Name.ToLowerInvariant()}"; // PostgreSQL prefers lowercase parameter names
                        parametersList.Add(paramName);
                        dapperParameters.Add(paramName, prop.Value ?? null);
                    }

                    query.Append(string.Join(", ", parametersList));
                }

                query.Append(')');

                // Log the query and parameters (for debugging)
                //if (_logger != null)
                //{
                //    _logger.LogDebug($"Generated Query: {query}");
                //    foreach (var param in dapperParameters.ParameterNames)
                //    {
                //        _logger.LogDebug($"Parameter: {param}, Value: {dapperParameters.Get<dynamic>(param)}");
                //    }
                //}

                var commandDefinition = new CommandDefinition(
                    commandText: query.ToString(),
                    parameters: dapperParameters,
                    commandType: CommandType.Text,
                    commandTimeout: 30, // Configurable timeout
                    cancellationToken: default
                );

                result = (List<TDTO>)await dbConnection.QueryAsync<TDTO>(commandDefinition);
                return result?.ToList() ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                //_logger?.LogError(ex, "Error executing function {FunctionName}", functionName);
                //throw new DatabaseException($"Error executing function {functionName}", ex);
            }
            return result;
        }
    }

    //Created for Generic repo refs
    public class RepositoryHelper
    {
        //public IDBRepository<X> GetRepository<X>(CMSDbContext dbContext) where X : class
        //{
        //    return new DBRepository<X>(dbContext);
        //}
    }
}
