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
}