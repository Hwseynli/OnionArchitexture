using OnionArchitecture.Domain.Common;
using OnionArchitecture.Infrastructure;

namespace OnionArchitecture.Domain.Entities;
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime LastPasswordChangeDateTime { get; private set; }
    public string? RefreshToken { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool Activated { get; private set; }

    public void SetDetails(string firstName, string lastName, string userName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Activated = true;
        IsDeleted = false;
        PasswordHash = PasswordHasher.HashPassword(password);
    }

    public void SetPasswordHash(string newPasswordHash)
    {
        if (PasswordHash != newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
        }
    }
    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
    }

    public void UpdateRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
