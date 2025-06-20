using Microsoft.EntityFrameworkCore.Storage;

namespace AlbinMicroService.DataMappers.EntityFramework
{
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();
        private IDbContextTransaction? _transaction;

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllNoTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<T> GetAllNoTracking(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Transaction Support
        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // === Savepoint Support Needed !! ===
        public async Task CreateSavepointAsync(string savepointName)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.CreateSavepointAsync(savepointName);
        }

        public async Task RollbackToSavepointAsync(string savepointName)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.RollbackToSavepointAsync(savepointName);
        }

        public async Task ReleaseSavepointAsync(string savepointName)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.ReleaseSavepointAsync(savepointName);
        }
    }
}
