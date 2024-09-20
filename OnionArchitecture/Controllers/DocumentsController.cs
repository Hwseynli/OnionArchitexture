using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Interfaces.IManagers;

namespace OnionArchitecture.Controllers;
[Route("api/document")]
[ApiController]
public class DocumentsController : Controller
{
    private readonly IDocumentManager _documentManager;

    public DocumentsController(IDocumentManager documentManager)
    {
        _documentManager = documentManager;
    }

    [HttpGet("{customerId}/documents/{additionDocumentId}")]
    public async Task<IActionResult> DownloadDocuments(int customerId, int additionDocumentId)
    {
        return await _documentManager.DownloadDocuments(customerId, additionDocumentId);
    }

    [HttpGet("{customerId}/documents")]
    public async Task<IActionResult> GetDocumentsWithTypes(int customerId)
    {
        return await _documentManager.GetDocumentsWithTypes(customerId);
    }
}

