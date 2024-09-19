using System.Security.Claims;
using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Application.Interfaces.IManagers;
public interface IClaimManager
{
    int GetCurrentUserId();
    IEnumerable<Claim> GetUserClaims(User user);
}
