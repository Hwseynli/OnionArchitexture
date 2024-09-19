using System.Linq.Expressions;

namespace OnionArchitecture.Application.Interfaces.IRepositories;
public interface IRepository<T> where T : class
{
    Task Commit(CancellationToken cancellationToken);
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task HardDelete(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task RemoveRange(IEnumerable<T> entities);
    Task Update(T entity);
}
