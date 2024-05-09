using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookShop.Services.Impl;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly JwtOptions _jwtOptions;
    public CustomAuthenticationService(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public JwtSecurityToken GenerateToken(ClientEntity clientEntity)
    {
        var claims = new List<Claim>
        {
            new Claim("clientId", clientEntity.Id.ToString()),
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );
        return token;
    }
}
