using BookShop.Services.Abstractions;
using BookShop.Services.Models;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authenticationService;

    public AuthenticationController(ICustomAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public ActionResult<TokenModel> Login(ClientLoginVm model)
    {
        var token = _authenticationService.GenerateToken(model);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }
}
