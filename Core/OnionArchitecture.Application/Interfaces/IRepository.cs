using System.Linq.Expressions;

namespace OnionArchitecture.Application.Interfaces;
public interface IRepository<T> where T : class
{
    Task Commit(CancellationToken cancellationToken);
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter=null);
    Task HardDeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
}
