using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Persistence.Context;
using System.Linq.Expressions;

namespace OnionArchitecture.Persistence.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly TestDbContext _context;

    public Repository(TestDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query.Include(include);
            }
        }
        return await (filter == null ? query.ToListAsync() : query.Where(filter).ToListAsync());
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query.Include(include);
            }
        }
        return await (filter == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(filter));
    }

    public void HardDelete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
