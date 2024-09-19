using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Application.Interfaces;
public interface IUserRepository : IRepository<User>
{
    Task<bool> IsUserNameUniqueAsync(string userName);
    Task<bool> IsEmailUniqueAsync(string email, int id = 0);

}
