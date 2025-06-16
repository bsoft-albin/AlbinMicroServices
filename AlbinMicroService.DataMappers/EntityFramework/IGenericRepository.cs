namespace AlbinMicroService.DataMappers.EntityFramework
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        IQueryable<T> GetAllNoTracking();
    }
}
