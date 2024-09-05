using System;
using MediatR;
using OnionArchitecture.Application.Interfaces;

namespace OnionArchitecture.Application.Features.Commands.User.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User();
        user.SetDetails(request.FirstName,request.LastName,request.UserName,request.Email,request.Password);
        await _userRepository.AddAsync(user);
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}

