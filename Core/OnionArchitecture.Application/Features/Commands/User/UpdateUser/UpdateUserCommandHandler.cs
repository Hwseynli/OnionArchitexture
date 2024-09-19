using MediatR;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Interfaces;

namespace OnionArchitecture.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.Id);

        if (user == null)
            new NotFoundException("User not found");

        // Parolun düzgünlüyünü validator yoxlayır, ona görə burda ayrıca yoxlamaya ehtiyac yoxdur.
        // Username yalnız ayda bir dəfə dəyişdirilə bilər (artıq validator yoxlayıb).
        if (user.UserName != request.UserName &&
            (DateTime.UtcNow - user.LastPasswordChangeDateTime).TotalDays >= 30)
        {
            user.SetUserName(request.UserName);  // SetUserName metodundan istifadə edilir
        }

        user.SetDetails(request.Name, request.Surname, user.UserName, request.Email, user.PasswordHash); // Surname,Email və Name dəyişdirilir

        await _userRepository.Commit(cancellationToken);

        return true;
    }
}

