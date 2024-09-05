using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Persistence.Data;

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

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, nameof(id)) == id);
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



    #region sehvdeyiluzundu
    public async Task<IEnumerable<T>> GetAllAsync(
                 Expression<Func<T, bool>>? filter = null,
                 params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();
        // Apply the filter if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }
        // Apply includes for related entities
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        // Return the result with AsNoTracking for better performance
        return await query.AsNoTracking().ToListAsync();
    }
    #endregion
}