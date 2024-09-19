using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword.SendOtp;
public class SendOtpCommand : IRequest<bool>
{
    public string Email { get; set; }
}

