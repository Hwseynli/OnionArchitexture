using FluentValidation;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer
{
    public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Customer ID must be greater than zero.");
            RuleFor(command => command.Name).NotNull();//.WithMessage("Name is required.");
            RuleFor(command => command.Surname).NotNull();//.WithMessage("Surname is required.");
            RuleFor(command => command.Email).NotNull()//.WithMessage("Email is required.")
                                             .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(command => command.AdditionDocuments).Must(x => x.Count > 0 || x.Count == 0)
                                                         .WithMessage("Documents list must be provided.");

            RuleForEach(command => command.AdditionDocuments)
                .ChildRules(doc =>
                {
                    doc.RuleFor(x => x.DocumentTypeId).NotNull();//.WithMessage("Document Type is required.");
                    doc.RuleFor(x => x.Documents).Must(docs => docs == null || docs.Count > 0)
                                                 .WithMessage("At least one document is required.");

                    doc.When(x => x.DocumentTypeId == (int)DocumentType.Other, () =>
                    {
                        doc.RuleFor(x => x.Other).NotNull();//.WithMessage("Other description is required for this document type.");
                    });
                });

            RuleFor(command => command.DeletedDocuments).Must(x => x != null).WithMessage("Deleted documents list cannot be null.");
        }
    }
}

