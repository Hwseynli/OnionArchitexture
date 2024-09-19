using OnionArchitecture.Application.Features.Queries.ViewModels;

namespace OnionArchitecture.Application.Features.Queries;
public interface IUserQueries
{
    Task<UserProfileDto> GetUserProfileAsync();

}
