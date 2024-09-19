using FluentValidation;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer
{
    public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(command => command.Id).NotNull().WithMessage("Customer Id is required.");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(command => command.Surname).NotEmpty().WithMessage("Surname is required.");
            RuleFor(command => command.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(command => command.AdditionDocuments).Must(x => x.Count > 0).WithMessage("At least one additional document is required.");

            RuleForEach(command => command.AdditionDocuments)
                .ChildRules(document =>
                {
                    document.RuleFor(x => x.DocumentTypeId).NotNull().WithMessage("Document type is required.");
                    document.RuleFor(x => x.Documents).Must(x => x.Count > 0).WithMessage("At least one document file is required.");
                    document.When(x => x.DocumentTypeId == (int)DocumentType.Other, () =>
                    {
                        document.RuleFor(x => x.Other).NotEmpty().WithMessage("Other document type details must be provided.");
                    });
                });
        }
    }
}

