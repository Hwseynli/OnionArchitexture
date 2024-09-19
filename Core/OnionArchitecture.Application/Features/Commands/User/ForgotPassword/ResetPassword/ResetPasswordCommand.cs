using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword.ResetPassword;
public class ResetPasswordCommand : IRequest<bool>
{
    public string NewPassword { get; set; }
    public string OtpCode { get; set; }
}