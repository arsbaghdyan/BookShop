using BookShop.Api.Constants;
using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route(Routes.Admin)]
public class EmployeeController : BaseAdminAuthorizedController
{
    private readonly IShopAuthenticationService _authenticationService;

    public EmployeeController(IShopAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenModel>> Login(EmployeeLoginModel model)
    {
        // TODO Refactor. Create and Use IEmployeeService
        var empl = new EmployeeModel
        {
            Id = 1,
            Email = model.Email,
        };

        var token = _authenticationService.GenerateAdminToken(empl);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }

    // CreateEmployee

    // UpdateEmployee

    // RemoveEmployee
}
