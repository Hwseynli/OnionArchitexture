using FluentValidation;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer
{
    public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            // Validate that ID is provided
            RuleFor(command => command.Id).GreaterThan(0);

            // Only validate name, surname, and email if they are not null (they might not change)
            When(command => !string.IsNullOrEmpty(command.Name), () =>
            {
                RuleFor(command => command.Name).NotNull().NotEmpty();
            });

            When(command => !string.IsNullOrEmpty(command.Surname), () =>
            {
                RuleFor(command => command.Surname).NotNull().NotEmpty();
            });

            When(command => !string.IsNullOrEmpty(command.Email), () =>
            {
                RuleFor(command => command.Email).NotNull().EmailAddress();
            });

            // Validate documents only if new documents are added or existing ones are updated
            When(command => command.AdditionDocuments.Any(), () =>
            {
                RuleForEach(command => command.AdditionDocuments).ChildRules(doc =>
                {
                    doc.RuleFor(d => d.DocumentTypeId).NotNull();
                    doc.RuleFor(d => d.Documents).Must(docs => docs.Count > 0).WithMessage("At least one document should be uploaded.");
                });
            });

            // Validate deleted documents if any are present
            When(command => command.DeletedDocuments.Any(), () =>
            {
                RuleFor(command => command.DeletedDocuments).Must(ids => ids.All(id => id > 0)).WithMessage("Invalid document ID for deletion.");
            });
        }
    }
}

