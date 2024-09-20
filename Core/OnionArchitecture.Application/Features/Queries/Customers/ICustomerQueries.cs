using OnionArchitecture.Application.Features.Queries.ViewModels.Customers;

namespace OnionArchitecture.Application.Features.Queries.Customers;
public interface ICustomerQueries
{
    Task<CustomerDto> GetByIdAsync(int customerId);
}

