using BookShop.Common.ClientService.Abstractions;
using BookShop.Common.Consts;
using System.IdentityModel.Tokens.Jwt;

namespace BookShop.Api.Middlewares;

public class ClientContextMiddleware : IMiddleware
{
    private readonly IClientContextWriter _clientContextWriter;

    public ClientContextMiddleware(IClientContextWriter clientContextWriter)
    {
        _clientContextWriter = clientContextWriter;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isAuthenticated = context.User.Identity?.IsAuthenticated;

        if (isAuthenticated == true)
        {
            var tokenHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(tokenHeader))
            {
                throw new Exception("Token is missing");
            }
            var token = tokenHeader.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);

            var clientIdClaim = securityToken.Claims.FirstOrDefault(c => c.Type == BookShopClaims.ClientId);

            if (clientIdClaim == null)
            {
                throw new Exception("clientId is missing");
            }

            if (!long.TryParse(clientIdClaim.Value, out long clientId))
            {
                throw new Exception("Unknown clientId");
            }

            _clientContextWriter.SetClientContextId(clientId);
        }

        await next(context);
    }
}
