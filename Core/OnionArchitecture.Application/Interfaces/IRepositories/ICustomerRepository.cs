using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Application.Interfaces.IRepositories;
public interface ICustomerRepository:IRepository<Customer>
{
    Task<Customer?> GetByIdAsync(int customerId);  // Müştərinin ID-si ilə məlumatları gətirir
}