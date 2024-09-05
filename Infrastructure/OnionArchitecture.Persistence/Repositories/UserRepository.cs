using System;
using OnionArchitecture.Application.Interfaces;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Persistence.Data;

namespace OnionArchitecture.Persistence.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TestDbContext context) : base(context)
    {
    }
}

