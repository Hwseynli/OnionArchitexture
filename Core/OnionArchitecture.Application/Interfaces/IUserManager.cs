using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Application.Interfaces;
public interface IUserManager
{
    public int GetCurrentUserId();
    (string token,DateTime expireAt) GenerateTJwtToken(User user);
}
