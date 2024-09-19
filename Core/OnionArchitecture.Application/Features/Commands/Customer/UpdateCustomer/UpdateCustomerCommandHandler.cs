using MediatR;
using Microsoft.Extensions.Options;
using OnionArchitecture.Application.Interfaces.IManagers;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;
using OnionArchitecture.Infrastructure.Extensions;
using OnionArchitecture.Infrastructure.Settings;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;
    private readonly IAdditionDocumentRepository _additionDocumentRepository;
    private readonly IDocumentRepository _documentRepository;
    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUserManager userManager, IOptions<FileSettings> fileSettings, IAdditionDocumentRepository additionDocumentRepository, IDocumentRepository documentRepository)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
        _additionDocumentRepository = additionDocumentRepository;
        _documentRepository = documentRepository;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var customer = await _customerRepository.GetAsync(x => x.Id == request.Id);
        customer.SetDetails(request.Name, request.Surname, request.Email);
        customer.SetEditFields(userId);
        for (int i = 0; i < request.AdditionDocuments.Count; i++)
        {
            if (request.AdditionDocuments[i].Id == 0)
            {
                AdditionDocument additionDocument = new AdditionDocument();
                additionDocument.SetDetails((DocumentType)request.AdditionDocuments[i].DocumentTypeId, request.AdditionDocuments[i].Other);
                additionDocument.SetAuditDetails(userId);
                for (int j = 0; j < request.AdditionDocuments[i].Documents.Count; j++)
                {
                    (string path, string fileName) = await request.AdditionDocuments[i].Documents[j].SaveAsync(_fileSettings.Value.CreateSubFolders(
                         _fileSettings.Value.Path,
                         _fileSettings.Value.CustomerSettings.EntityName,
                         request.Email,
                         _fileSettings.Value.CustomerSettings.Documents)
                        );

                    var document = new Document();
                    document.SetDetails(fileName, path);
                    document.SetAuditDetails(userId);
                    additionDocument.Documents.Add(document);
                }
                customer.AdditionDocuments.Add(additionDocument);

            }
            else
            {
                var existDocuments = await _additionDocumentRepository.GetAsync(x => x.Id == request.AdditionDocuments[i].Id);
                existDocuments.SetDetails((DocumentType)request.AdditionDocuments[i].DocumentTypeId, request.AdditionDocuments[i].Other);
                existDocuments.SetEditFields(userId);
                for (int j = 0; j < request.AdditionDocuments[i]?.Documents?.Count; j++)
                {
                    (string path, string fileName) = await request.AdditionDocuments[i].Documents[j].SaveAsync(_fileSettings.Value.CreateSubFolders(
                         _fileSettings.Value.Path,
                         _fileSettings.Value.CustomerSettings.EntityName,
                         request.Email,
                         _fileSettings.Value.CustomerSettings.Documents)
                        );

                    var document = new Document();
                    document.SetDetails(fileName, path);
                    document.SetAuditDetails(userId);
                    existDocuments.Documents.Add(document);
                }
                customer.AdditionDocuments.Add(existDocuments);
            }
        }

        if (request.DeletedDocuments != null)
        {
            var documents = await _documentRepository.GetAllAsync(x => request.DeletedDocuments.Contains(x.Id));
           await _documentRepository.RemoveRange(documents);
        }
        await _customerRepository.Update(customer);
        await _customerRepository.Commit(cancellationToken);
        return true;
    }
}