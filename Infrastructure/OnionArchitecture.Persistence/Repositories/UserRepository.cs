using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TestDbContext context) : base(context)
    {
    }

    public async Task<bool> IsUserNameUniqueAsync(string userName)
    {
        var existingUser = await GetAsync(u => u.UserName == userName);
        return existingUser == null;
    }
    public async Task<bool> IsEmailUniqueAsync(string email, int id = 0) // Yeni metod
    {
        if (id == 0)
        {
            var existingUser = await GetAsync(u => u.Email == email);
            return existingUser == null;
        }
        else
        {
            var existingUser = await GetAsync(u => u.Email == email && u.Id != id);
            return existingUser == null;
        }
    }
}
