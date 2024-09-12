using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.UpdatePasswordWithOtp;
public class UpdatePasswordWithOtpCommand : IRequest<bool>
{
    public string OtpCode { get; set; }
    public string NewPassword { get; set; }
}

