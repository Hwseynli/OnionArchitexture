using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;
public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        // Validate that ID is provided
        RuleFor(command => command.Id).GreaterThan(0);

        // Only validate name, surname, and email if they are not null (they might not change)
            RuleFor(command => command.Name).NotNull().NotEmpty();

            RuleFor(command => command.Surname).NotNull().NotEmpty();

            RuleFor(command => command.Email).NotNull().EmailAddress();

        // Validate deleted documents if any are present
        When(command => command.DeletedDocuments.Any(), () =>
        {
            RuleFor(command => command.DeletedDocuments).Must(ids => ids.All(id => id > 0)).WithMessage("Invalid document ID for deletion.");
        });
    }
}

