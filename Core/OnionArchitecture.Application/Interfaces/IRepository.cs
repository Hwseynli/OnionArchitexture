using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Commit();
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter=null);
        Task HardDeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);

    }

   
   


}
