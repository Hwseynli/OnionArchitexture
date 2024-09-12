using FluentValidation;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Infrastructure;

namespace OnionArchitecture.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(command => command.Name).NotNull();
        RuleFor(command => command.Surname).NotNull();
        RuleFor(command => command.Email).Must(x => x.Contains("@gmail.com"))
            .WithMessage("Sadəcə daxili mail olmalıdır");

        RuleFor(command => command.UserName).NotNull().MustAsync(async (command, userName, cancellation) =>
        {
            var user = await _userRepository.GetAsync(u => u.Id == command.Id);

            // İstifadəçi Username-i ayda yalnız bir dəfə dəyişə bilər.
            if (user != null && user.UserName != userName &&
                (DateTime.UtcNow - user.LastPasswordChangeDateTime).TotalDays < 30)
            {
                return false;
            }
            return true;
        }).WithMessage("Username yalnız ayda bir dəfə dəyişdirilə bilər.");

        // Mövcud parolun düzgünlüyünü yoxlamaq üçün
        RuleFor(command => command.Password).NotNull()
            .MustAsync(async (command, password, cancellation) =>
            {
                var user = await _userRepository.GetAsync(u => u.Id == command.Id);
                return user != null && user.PasswordHash == PasswordHasher.HashPassword(password);
            }).WithMessage("Mövcud parol səhvdir.");
    }
}

