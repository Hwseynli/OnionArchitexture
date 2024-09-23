﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class CustomerRepository:Repository<Customer>,ICustomerRepository
{
    private readonly TestDbContext _context;

    public CustomerRepository(TestDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers
            .Include(c => c.AdditionDocuments)   // Müştəriyə aid sənədləri daxil etmək üçün
            .ThenInclude(ad => ad.Documents)     // Hər bir sənədin detalları ilə birlikdə
            .FirstOrDefaultAsync(c => c.Id == customerId);  // Müştərinin ID-si əsasında gətiririk
    }

    public async Task<IEnumerable<Customer>> GetAllPagedAsync(int pageNumber, int pageSize, Expression<Func<Customer, bool>>? filter = null)
    {
        IQueryable<Customer> query = _context.Customers
            .Include(c => c.AdditionDocuments)
            .ThenInclude(ad => ad.Documents);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetAllCountAsync(Expression<Func<Customer, bool>>? filter = null)
    {
        IQueryable<Customer> query = _context.Customers;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync();
    }
}
