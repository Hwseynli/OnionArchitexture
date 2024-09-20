using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Queries.ViewModels.Customers;
using OnionArchitecture.Application.Interfaces.IRepositories;

namespace OnionArchitecture.Application.Features.Queries.Customers;
public class CustomerQueries:ICustomerQueries
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAdditionDocumentRepository _additionDocumentRepository;

    public CustomerQueries(ICustomerRepository customerRepository, IAdditionDocumentRepository additionDocumentRepository)
    {
        _customerRepository = customerRepository;
        _additionDocumentRepository = additionDocumentRepository;
    }

    public async Task<CustomerDto> GetByIdAsync(int customerId)
    {
        // Müştərinin ID-si ilə məlumatlarını əldə edirik
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new NotFoundException("Customer not found");

        // Müştərinin sənədlərini əldə edirik
        var documents = await _additionDocumentRepository.GetDocumentsByCustomerIdAsync(customerId);

        // Müştərinin məlumatlarını DTO olaraq qaytarırıq
        // Customer məlumatlarını Dto-ya çeviririk
        var customerDto = new CustomerDto
        {
            CustomerId = customer.Id,
            FirstName = customer.FirstName,
            LastName=customer.LastName,
            Email = customer.Email,
            // digər sahələr...
            AdditionDocuments = customer.AdditionDocuments?.Select(ad => new DocumentDto
            {
                DocumentId = ad.Id,
                DocumentName = ad.Documents.FirstOrDefault()?.Name ?? "Unnamed", // İlk sənədin adı
                DocumentType = ad.DocumentType.ToString()  // Sənədin tipi
            }).ToList()
        };

        return customerDto;
    }

    public async Task<IActionResult> GetCustomerDocuments(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return new NotFoundResult();
        }

        var customerDocuments = customer.AdditionDocuments
            .SelectMany(ad => ad.Documents.Select(d => new
            {
                DocumentId = d.Id,
                DocumentName = d.Name,
                DocumentPath = d.Path,
                DocumentType = ad.DocumentType.ToString()
            }))
            .ToList();

        return new OkObjectResult(customerDocuments);
    }

}