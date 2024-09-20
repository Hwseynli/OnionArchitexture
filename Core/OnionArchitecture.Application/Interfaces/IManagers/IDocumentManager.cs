using Microsoft.AspNetCore.Mvc;

namespace OnionArchitecture.Application.Interfaces.IManagers;
public interface IDocumentManager
{
    Task<IActionResult> DownloadDocuments(int customerId, int additionDocumentId);
    Task<IActionResult> GetDocumentsWithTypes(int customerId);
}
