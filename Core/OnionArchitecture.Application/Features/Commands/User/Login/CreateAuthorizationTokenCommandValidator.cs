using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommandValidator:AbstractValidator<CreateAuthorizationTokenCommand>
{
    public CreateAuthorizationTokenCommandValidator()
    {
        RuleFor(command => command.Username).NotNull();
        RuleFor(command => command.Password).MinimumLength(6).NotNull();
    }
}
