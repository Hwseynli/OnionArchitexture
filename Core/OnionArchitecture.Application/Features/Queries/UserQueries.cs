using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Queries.User;
using OnionArchitecture.Application.Interfaces;

namespace OnionArchitecture.Application.Features.Queries;
public class UserQueries : IUserQueries
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UserQueries(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }
    public async Task<UserProfileDto> GetUserProfileAsync()
    {
        //var id = _userManager.GetCurrentUserId();
        var user = await _userRepository.GetAsync((u => u.Id == _userManager.GetCurrentUserId()));
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        return new UserProfileDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email
        };
    }
}
