﻿using OnionArchitecture.Domain.Common;
using OnionArchitecture.Infrastructure.Utils;

namespace OnionArchitecture.Domain.Entities;
public class User : IBaseEntity
{
    public int Id { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime LastPasswordChangeDateTime { get; private set; }
    public string? RefreshToken { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool Activated { get; private set; }
    //ForgotPassword üçün :
    public string? OtpCode { get; private set; }
    public DateTime?OtpGeneratedAt { get; private set; }

    public void SetDetailsForRegister(string firstName, string lastName, string userName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Activated = true;
        IsDeleted = false;
        PasswordHash = PasswordHasher.HashPassword(password);
    }

    public void SetDetailsForUpdate(string firstName, string lastName, string userName, string email, string passwordHash)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Activated = true;
        IsDeleted = false;
        PasswordHash = passwordHash;
    }

    public void SetPasswordHash(string newPasswordHash)
    {
        if (PasswordHash != newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
        }
    }

    public void UpdateRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
    //Update üçün:
    public void SetUserName(string userName)
    {
        UserName = userName;
    }
    //ForgotPassword üçün:
    public void UpdateOtp(string? otpCode)
    {
        OtpCode = otpCode;
        OtpGeneratedAt = DateTime.UtcNow.AddHours(4);
    }

    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
    }
}
