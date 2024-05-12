using BookShop.Common.Consts;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.ClientModels;
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

    public string GenerateToken(ClientModel client)
    {
        var claims = new List<Claim>
        {
            new Claim(BookShopClaims.Id, client.Id.ToString()),
            new Claim(ClaimTypes.Email, client.Email)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
