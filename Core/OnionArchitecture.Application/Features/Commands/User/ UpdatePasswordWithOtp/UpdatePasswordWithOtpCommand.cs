using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.UpdatePasswordWithOtp;
public class UpdatePasswordWithOtpCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string OtpCode { get; set; }
    public string NewPassword { get; set; }
}

