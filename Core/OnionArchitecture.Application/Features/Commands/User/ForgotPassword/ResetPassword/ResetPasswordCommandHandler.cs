using MediatR;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Infrastructure.Utils;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword.ResetPassword;
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public ResetPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.OtpCode != null && u.OtpCode == request.OtpCode && u.OtpGeneratedAt != null);

        if (user == null)
            throw new BadRequestException("Invalid OTP or OTP has expired.");

        user.ResetPassword(PasswordHasher.HashPassword(request.NewPassword));
        user.UpdateOtp(null); // OTP-i sil
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}