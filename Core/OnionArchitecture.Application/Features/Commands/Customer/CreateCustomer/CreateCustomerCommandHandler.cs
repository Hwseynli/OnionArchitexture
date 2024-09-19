using MediatR;
using Microsoft.Extensions.Options;
using OnionArchitecture.Application.Interfaces.IManagers;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;
using OnionArchitecture.Infrastructure.Extensions;
using OnionArchitecture.Infrastructure.Settings;

namespace OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
public class CreateCustomerCommandHandler: IRequestHandler<CreateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
    }

    public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var customer = new Domain.Entities.Customer();
        customer.SetDetails(request.Name, request.Surname, request.Email);
        customer.SetAuditDetails(userId);
        for (int i = 0; i < request.AdditionDocuments.Count; i++)
        {
            AdditionDocument additionDocument = new AdditionDocument();
            additionDocument.SetDetails((DocumentType)request.AdditionDocuments[i].DocumentTypeId, request.AdditionDocuments[i].Other);
            additionDocument.SetAuditDetails(userId);
            for (int j = 0; j < request.AdditionDocuments[i].Documents.Count; j++)
            {
                (string path, string fileName) = await request.AdditionDocuments[i].Documents[j].SaveAsync(_fileSettings.Value.CreateSubFolfers(
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
        await _customerRepository.AddAsync(customer);
        await _customerRepository.Commit(cancellationToken);
        return true;
    }
}

