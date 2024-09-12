using FluentValidation;
using OnionArchitecture.Application.Interfaces;

namespace OnionArchitecture.Application.Features.Commands.User.Register;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandValidator(IUserRepository userRepository) : base()
    {
        _userRepository = userRepository;

        RuleFor(command => command.Name).NotNull();
        RuleFor(command => command.UserName).NotNull()
            .MustAsync(async (userName, cancellation) =>
                await _userRepository.IsUserNameUniqueAsync(userName))
                .WithMessage("Username artıq mövcuddur");
        RuleFor(command => command.Surname).NotNull();
        RuleFor(command => command.Password).MinimumLength(6).NotNull();
        RuleFor(command => command.Email).NotNull()
            .MustAsync(async (email, cancellation) =>
                    await _userRepository.IsEmailUniqueAsync(email))
                .WithMessage("Email artıq mövcuddur")
            .Must(x => x.Contains("@gmail.com"))
                .WithMessage("Sadəcə daxili mail olmalıdır");
    }
}


