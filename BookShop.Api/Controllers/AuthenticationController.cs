using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class AuthenticationController : BaseClientAuthorizedController
{
    private readonly IShopAuthenticationService _authenticationService;
    private readonly IClientService _clientService;

    public AuthenticationController(IShopAuthenticationService authenticationService,
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

        var token = _authenticationService.GenerateClientToken(client);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }
}
