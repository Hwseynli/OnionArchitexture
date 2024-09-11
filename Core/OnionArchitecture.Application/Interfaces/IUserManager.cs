using OnionArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Interfaces
{
    public interface IUserManager
    {
        public int GetCurrentUserId();
        (string token,DateTime expireAt) GenerateTJwtToken(User user);
    }
}
