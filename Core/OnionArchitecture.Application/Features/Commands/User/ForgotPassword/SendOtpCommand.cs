using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword;
public class SendOtpCommand : IRequest<bool>
{
    public string Email { get; set; }
}

