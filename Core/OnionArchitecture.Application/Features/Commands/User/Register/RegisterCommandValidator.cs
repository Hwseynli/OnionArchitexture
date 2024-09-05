using System;
using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.User.Register;
public class RegisterCommandValidator:AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator():base()
    {
        RuleFor(command => command.FirstName).NotNull();
        RuleFor(command => command.LastName).NotNull();
        RuleFor(command => command.Email).Must(
            x=>x.Contains("@gmail.com"))
                .WithMessage("Sadece daxili mail olmalıdır. ");

        RuleFor(command => command.Password).MinimumLength(6).NotNull();


    }
}

