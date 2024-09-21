using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Queries.ViewModels.Customers;

namespace OnionArchitecture.Application.Features.Queries.Customers;
public interface ICustomerQueries
{
    Task<CustomerDto> GetByIdAsync(int customerId);
    Task<IActionResult> GetCustomerDocuments(int customerId);
    Task<IActionResult> DownloadDocuments(int additionDocumentId);
    Task<IActionResult> GetDocumentsWithTypes(int customerId);
}

