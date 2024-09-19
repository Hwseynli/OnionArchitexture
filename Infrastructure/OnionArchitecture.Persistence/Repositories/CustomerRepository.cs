using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class CustomerRepository:Repository<Customer>,ICustomerRepository
{
    public CustomerRepository(TestDbContext context) : base(context)
    {
    }
}
