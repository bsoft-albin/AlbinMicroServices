namespace AlbinMicroService.DataMappers.EntityFramework
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllNoTracking();
        IQueryable<T> GetAllNoTracking(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> predicate);
        Task ReleaseSavepointAsync(string savepointName);
        Task RollbackToSavepointAsync(string savepointName);
        Task CreateSavepointAsync(string savepointName);
    }
}
