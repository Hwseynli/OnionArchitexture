using System;
using MediatR;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Infrastructure;

namespace OnionArchitecture.Application.Features.Commands.User.UpdatePasswordWithOtp;
public class UpdatePasswordWithOtpCommandHandler : IRequestHandler<UpdatePasswordWithOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdatePasswordWithOtpCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdatePasswordWithOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.OtpCode != request.OtpCode || user.OtpGeneratedAt == null || user.OtpGeneratedAt.Value.AddMinutes(15) < DateTime.UtcNow)
        {
            throw new BadRequestException("Invalid OTP or OTP has expired.");
        }

        user.ResetPassword(PasswordHasher.HashPassword(request.NewPassword));
        user.UpdateOtp(null); // OTP-i sil
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}
