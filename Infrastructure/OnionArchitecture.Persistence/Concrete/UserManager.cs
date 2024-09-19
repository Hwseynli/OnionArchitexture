﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnionArchitecture.Application.Interfaces.IManagers;
using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Persistence.Concrete;
public class UserManager : IUserManager
{
    private readonly IClaimManager _claimManager;
    private readonly IConfiguration _configuration;

    public UserManager(IClaimManager claimManager, IConfiguration configuration)
    {
        _claimManager = claimManager;
        _configuration = configuration;
    }

    public (string token, DateTime expireAt) GenerateTJwtToken(User user)
    {
        var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
        claims.AddRange(_claimManager.GetUserClaims(user));
        var jwtSettings = _configuration.GetSection("JWTSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
        var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireAt = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireAt"]));
        var token = new JwtSecurityToken
            (claims: claims,
            expires: expireAt,
            signingCredentials: creadentials
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        return (tokenHandler.WriteToken(token), expireAt);
    }

    public int GetCurrentUserId()
    {
        return _claimManager.GetCurrentUserId();
    }
}
