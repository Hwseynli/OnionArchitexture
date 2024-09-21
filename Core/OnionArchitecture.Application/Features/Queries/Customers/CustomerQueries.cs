using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Queries.ViewModels.Customers;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Features.Queries.Customers;
public class CustomerQueries:ICustomerQueries
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAdditionDocumentRepository _additionDocumentRepository;
    private readonly IDocumentRepository _documentRepository;

    public CustomerQueries(ICustomerRepository customerRepository, IAdditionDocumentRepository additionDocumentRepository, IDocumentRepository documentRepository)
    {
        _customerRepository = customerRepository;
        _additionDocumentRepository = additionDocumentRepository;
        _documentRepository = documentRepository;
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
            throw new NotFoundException(); 

        var customerDocuments = customer.AdditionDocuments
            .SelectMany(ad => ad.Documents.Select(d => new
            {
                DocumentId = d.Id,
                DocumentName = d.Name,
                DocumentType = ad.DocumentType.ToString()
            }))
            .ToList();

        return new OkObjectResult(customerDocuments);
    }

    public async Task<IActionResult> DownloadDocuments(int additionDocumentId)
    {
        var documents = await _documentRepository.GetAllAsync(d => d.AdditionDocumentId == additionDocumentId);

        if (!documents.Any())
            return new NotFoundResult();

        if (documents.Count() == 1)
        {
            var document = documents.First();
            var fileStream = new FileStream(document.Path, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(document.Path).TrimStart('.');
            return new FileStreamResult(fileStream, $"application/{extension}")
            {
                FileDownloadName = document.Name
            };
        }
        else
        {
            var zipFile = await CreateZipArchive(documents);
            var zipStream = new FileStream(zipFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(zipStream, "application/zip")
            {
                FileDownloadName = $"{additionDocumentId}_documents.zip"
            };
        }
    }

    public async Task<IActionResult> GetDocumentsWithTypes(int customerId) //lazim deyil
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return new NotFoundResult();
        }

        var documentsWithTypes = customer.AdditionDocuments
            .SelectMany(ad => ad.Documents.Select(d => new
            {
                DocumentId = d.Id,
                DocumentType = ad.DocumentType.ToString(),
                d.Name
            }))
            .ToList();

        return new OkObjectResult(documentsWithTypes);
    }

    private async Task<string> CreateZipArchive(IEnumerable<Document> documents)
    {
        var zipFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");

        using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
        {
            foreach (var document in documents)
            {
                zipArchive.CreateEntryFromFile(document.Path, document.Name);
            }
        }

        return zipFilePath;
    }
}