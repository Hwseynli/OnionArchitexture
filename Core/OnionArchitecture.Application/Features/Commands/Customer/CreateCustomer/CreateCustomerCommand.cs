using MediatR;
using OnionArchitecture.Application.Features.Commands.Customer.ViewModels;

namespace OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
public class CreateCustomerCommand:IRequest<bool>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<AdditionDocumentModel> AdditionDocuments { get; set; }
}


