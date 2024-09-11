using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.User.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator():base()
        {
            RuleFor(command=>command.Name).NotNull();
            RuleFor(command=>command.Surname).NotNull();
            RuleFor(command=>command.Password).MinimumLength(10).NotNull();
            RuleFor(command=>command.Email).Must(x=>
            x.Contains("@gmail.com")).WithMessage("Sadəcə daxili mail olmalıdır");

        }
    }
}
