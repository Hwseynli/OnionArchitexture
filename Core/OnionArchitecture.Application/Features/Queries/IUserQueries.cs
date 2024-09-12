using OnionArchitecture.Application.Features.Queries.User;

namespace OnionArchitecture.Application.Features.Queries;
public interface IUserQueries
{
    Task<UserProfileDto> GetUserProfileAsync(string email);

}
