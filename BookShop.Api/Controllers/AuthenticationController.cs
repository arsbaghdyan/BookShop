using BookShop.Services.Abstractions;
using BookShop.Services.Models;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authenticationService;
    private readonly IClientService _clientService;

    public AuthenticationController(ICustomAuthenticationService authenticationService,
        IClientService clientService)
    {
        _authenticationService = authenticationService;
        _clientService = clientService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenModel>> Login(ClientLoginModel model)
    {
        var client = await _clientService.GetByEmailAndPasswordAsync(
            model.Email,
            model.Password);

        if (client == null)
        {
            return Unauthorized();
        }

        var token = _authenticationService.GenerateToken(client);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }
}
