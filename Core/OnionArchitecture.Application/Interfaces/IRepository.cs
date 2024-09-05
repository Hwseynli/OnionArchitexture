using System;
using System.Linq.Expressions;

namespace OnionArchitecture.Application.Interfaces;
public interface IRepository<T> where T : class
{
    Task Commit(CancellationToken cancellationToken);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    void DeleteAsync(T entity);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

    public Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);

    #region sehvdeyiluzundu
    public Task<IEnumerable<T>> GetAllAsync(
                 Expression<Func<T, bool>>? filter = null,
                 params Expression<Func<T, object>>[] includes);
    #endregion
}
