using System.Linq.Expressions;
using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
