using System.Linq.Expressions;

namespace DataTypes.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByUuIdAsync(Guid uuid);
        Task<T?> GetByIdAndUuIdAsync(int id, Guid uuid);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
