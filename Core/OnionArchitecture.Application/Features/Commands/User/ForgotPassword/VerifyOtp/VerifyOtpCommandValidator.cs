using FluentValidation;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
public class VerifyOtpCommandValidator:AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.OtpCode).NotEmpty();
    }
}

