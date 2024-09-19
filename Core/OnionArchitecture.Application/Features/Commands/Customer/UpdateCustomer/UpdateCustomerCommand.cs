using MediatR;
using OnionArchitecture.Application.Features.Commands.Customer.ViewModels;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;
public class UpdateCustomerCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<AdditionUpdateDocumentModel> AdditionDocuments { get; set; } = new List<AdditionUpdateDocumentModel> { };

    public List<int> DeletedDocuments { get; set; } = new List<int> { };
}

