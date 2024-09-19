using FluentValidation;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(command => command.Name).NotNull();
        RuleFor(command => command.Surname).NotNull();
        RuleFor(command => command.Email).NotNull();
        RuleFor(command => command.AdditionDocuments).Must(x => x.Count > 0);
        RuleForEach(command => command.AdditionDocuments)
            .ChildRules(order =>
            {
                order.RuleFor(x => x.DocumentTypeId).NotNull();
                order.RuleFor(x => x.Documents).Must(x => x.Count > 0);
                order.When(x => x.DocumentTypeId == (int)DocumentType.Other, () =>
                {
                    order.RuleFor(x => x.Other).NotNull();
                });
            });
    }
}