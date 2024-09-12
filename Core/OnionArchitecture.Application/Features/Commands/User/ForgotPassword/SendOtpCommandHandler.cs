﻿using FluentValidation;
using MediatR;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Infrastructure.Services;
using OnionArchitecture.Infrastructure.Utils;

namespace OnionArchitecture.Application.Features.Commands.User.ForgotPassword;
public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IValidator<SendOtpCommand> _validator;

    public SendOtpCommandHandler(IUserRepository userRepository, IEmailService emailService, IValidator<SendOtpCommand> validator)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _validator = validator;
    }

    public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new Exceptions.ValidationException(validationResult.Errors);

        var user = await _userRepository.GetAsync(u => u.Email == request.Email);
        if (user == null)
            throw new NotFoundException("User not found.");

        var otpCode = OtpGenerator.GenerateOtp(); // OTP yaradılır
        user.UpdateOtp(otpCode); // OTP user obyektində saxlanılır (məsələn, baza üçün)
        await _userRepository.Commit(cancellationToken);

        await _emailService.SendOtpAsync(user.Email, otpCode); // OTP email ilə göndərilir
        return true;
    }
}

