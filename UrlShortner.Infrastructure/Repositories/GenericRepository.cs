using UrlShortner.Domain;
using UrlShortner.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAcessEFCore.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {
        private readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return null;
            // return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync( x => x.Id == id);
        }

        public async Task<T?> GetByUuIdAsync(Guid uuid)
        {
            return null;
            // return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.UuId == uuid);
        }

        public async Task<T?> GetByIdAndUuIdAsync(int id, Guid uuid)
        {
            return null;
            // return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UuId == uuid);
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity is DateTimeStamp ent)
            {
                ent.CreatedAt = DateTime.UtcNow;
                ent.UpdatedAt = DateTime.UtcNow;
            }

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities); 
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity is DateTimeStamp ent)
            {
                ent.UpdatedAt = DateTime.UtcNow;
            }

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).AsNoTracking().ToListAsync();  
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken: token);
        }

        public async Task RemoveAsync (T entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Execute the operation (a delegate)
                await operation();

                // Commit if everything succeeds
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback on failure
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
