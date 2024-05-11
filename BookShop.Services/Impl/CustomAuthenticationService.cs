using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
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

    public string GenerateToken(ClientLoginVm client)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, client.Email),
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

    public string GetClientEmailFromToken(string token)
    {
        if (token != null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var clientEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

            return clientEmail;
        }
        throw new InvalidOperationException("Token not found.");
    }
}
