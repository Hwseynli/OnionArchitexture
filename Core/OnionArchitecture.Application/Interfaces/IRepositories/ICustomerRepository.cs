using System.Linq.Expressions;
using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Application.Interfaces.IRepositories;
public interface ICustomerRepository:IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetAllByDateRangeAsync(DateTime? fromDate, DateTime? toDate);
    Task<Customer?> GetByIdAsync(int customerId);  // Müştərinin ID-si ilə məlumatları gətirir
    Task<IEnumerable<Customer>> GetAllPagedAsync(int pageNumber, int pageSize, Expression<Func<Customer, bool>>? filter = null);
    Task<int> GetAllCountAsync(Expression<Func<Customer, bool>>? filter = null);
}