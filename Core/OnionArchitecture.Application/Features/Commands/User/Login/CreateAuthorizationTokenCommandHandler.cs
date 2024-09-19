using MediatR;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Commands.User.ViewModels;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Application.Interfaces.IManagers;
using System.Security.Cryptography;
using OnionArchitecture.Infrastructure.Utils;

namespace OnionArchitecture.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;
    public CreateAuthorizationTokenCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<JwtTokenDto> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == request.Username.ToLower());

        if (user == null
            || user.PasswordHash != PasswordHasher.HashPassword(request.Password)
            || !user.Activated
            || user.IsDeleted)

            throw new  UnAuthorizedException("Invalid credentials");

        var random = GenerateRandomNumber();
        var refreshToken = $"{random}_{user.Id}_{DateTime.UtcNow.AddDays(20)}";
        user.UpdateRefreshToken(refreshToken);
        (string token, DateTime expireAt) = _userManager.GenerateTJwtToken(user);
        await _userRepository.Commit(cancellationToken);
        return new JwtTokenDto
        {
            ExpireAt = expireAt,
            RefreshToken = refreshToken,
            Token = token
        };

    }

    private object GenerateRandomNumber()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
