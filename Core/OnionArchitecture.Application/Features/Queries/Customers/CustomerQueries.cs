using System.IO.Compression;
using System.Linq.Expressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Queries.ViewModels.Customers;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities;
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

    public async Task<byte[]> ExportCustomersToExcelAsync(DateTime? fromDate, DateTime? toDate)
    {
        // Müvafiq tarix aralığında olan müştəriləri əldə etmək
        var customers = await _customerRepository.GetAllByDateRangeAsync(fromDate, toDate);

        // Excel faylı yaratmaq üçün MemoryStream istifadə olunur
        using (var memoryStream = new MemoryStream())
        {
            // Excel sənədi yaratmaq
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Sheet yaratmaq
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Sheets kolleksiyasını yaratmaq
                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                // Sheet əlavə etmək
                Sheet sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Customers"
                };
                sheets.Append(sheet);

                // SheetData alınır
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Başlıqları əlavə edirik
                Row headerRow = new Row();
                headerRow.Append(
                    CreateCell("Customer ID"),
                    CreateCell("First Name"),
                    CreateCell("Last Name"),
                    CreateCell("Email"),
                    CreateCell("AdditionId"),
                    CreateCell("Document Name"),
                    CreateCell("Document Type"),
                    CreateCell("Document Path")
                );
                sheetData.AppendChild(headerRow);

                // Məlumatları əlavə etmək
                foreach (var customer in customers)
                {
                    foreach (var additionDocument in customer.AdditionDocuments)
                    {
                        foreach (var document1 in additionDocument.Documents)
                        {
                            Row dataRow = new Row();
                            dataRow.Append(
                                CreateCell(customer.Id.ToString()),
                                CreateCell(customer.FirstName),
                                CreateCell(customer.LastName),
                                CreateCell(customer.Email),
                                CreateCell(additionDocument.Id.ToString()),
                                CreateCell(document1.Name),
                                CreateCell(additionDocument.DocumentType.ToString()),
                                CreateCell(document1.Path)
                            );
                            sheetData.AppendChild(dataRow);
                        }
                    }
                }

                workbookPart.Workbook.Save();
            }

            return memoryStream.ToArray();
        }
    }

    // Hüceyrə yaratmaq üçün funksiya
    private Cell CreateCell(string value)
    {
        return new Cell()
        {
            DataType = CellValues.String,
            CellValue = new CellValue(value)
        };
    }

    // Customer üçün getAll yazın:pagination və filtrasiya ilə bir yerdə olsun.  Date görə də filtr olsun
    // Pagination və tarix filtrasiya üçün yeni metod
    public async Task<PagedResult<CustomerDto>> GetAllAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate)
    {
        // Tarix üzrə filtrasiya tətbiq olunur
        Expression<Func<Customer, bool>>? filter = null;

        if (fromDate.HasValue && toDate.HasValue)
        {
            filter = c => c.RecordDateTime >= fromDate && c.RecordDateTime <= toDate;
        }
        else if (fromDate.HasValue)
        {
            filter = c => c.RecordDateTime >= fromDate;
        }
        else if (toDate.HasValue)
        {
            filter = c => c.RecordDateTime <= toDate;
        }

        // Müştərilərin cəmi sayını əldə edirik (pagination üçün lazım olacaq)
        var totalRecords = await _customerRepository.GetAllCountAsync(filter);

        // Müştəriləri lazımi səhifə ölçüsünə və səhifə nömrəsinə görə alırıq
        var customers = await _customerRepository.GetAllPagedAsync(pageNumber, pageSize, filter);

        // Customer məlumatlarını DTO formatına çeviririk
        var customerDtos = customers.Select(customer => new CustomerDto
        {
            CustomerId = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            AdditionDocuments = customer.AdditionDocuments?.Select(ad => new DocumentDto
            {
                DocumentId = ad.Id,
                DocumentName = ad.Documents.FirstOrDefault()?.Name ?? "Unnamed",
                DocumentType = ad.DocumentType.ToString()
            }).ToList()
        }).ToList();

        return new PagedResult<CustomerDto>
        {
            Items = customerDtos,
            TotalCount = totalRecords,
            PageSize = pageSize,
            PageNumber = pageNumber
        };
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