using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Interfaces.IManagers;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Persistence.Concrete;
public class DocumentManager : IDocumentManager
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentManager(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<IActionResult> DownloadDocuments(int customerId, int additionDocumentId)
    {
        var documents = await _documentRepository.GetAllAsync(d => d.AdditionDocument.CustomerId == customerId && d.AdditionDocumentId == additionDocumentId);

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
                FileDownloadName = $"{customerId}_documents.zip"
            };
        }
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

