using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Queries.User;
using OnionArchitecture.Application.Interfaces;

namespace OnionArchitecture.Application.Features.Queries;
public class UserQueries : IUserQueries
{
    private readonly IUserRepository _userRepository;

    public UserQueries(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserProfileDto> GetUserProfileAsync(string email)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email);
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
