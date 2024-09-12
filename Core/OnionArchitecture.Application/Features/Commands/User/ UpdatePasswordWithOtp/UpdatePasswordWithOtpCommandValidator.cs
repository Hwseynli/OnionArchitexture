using System;
using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.User.UpdatePasswordWithOtp;
public class UpdatePasswordWithOtpCommandValidator : AbstractValidator<UpdatePasswordWithOtpCommand>
{
    public UpdatePasswordWithOtpCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.OtpCode).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}

