using BookShop.Api.Constants;
using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.EmployeeModels;
using BookShop.Services.Models.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route(Routes.Admin)]
public class EmployeeController : BaseAdminAuthorizedController
{
    private readonly IShopAuthenticationService _authenticationService;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IShopAuthenticationService authenticationService, IEmployeeService employeeService)
    {
        _authenticationService = authenticationService;
        _employeeService = employeeService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login/admin")]
    public async Task<ActionResult<TokenModel>> AdminLogin(EmployeeLoginModel model)
    {
        var employee = await _employeeService.GetAdminByEmailAndPasswordAsync(
            model.Email,
            model.Password);

        if (employee == null)
        {
            return Unauthorized();
        }

        var token = _authenticationService.GenerateAdminToken(employee);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login/employee")]
    public async Task<ActionResult<TokenModel>> EmployeeLogin(EmployeeLoginModel model)
    {
        var employee = await _employeeService.GetEmployeeByEmailAndPasswordAsync(
            model.Email,
            model.Password);

        if (employee == null)
        {
            return Unauthorized();
        }

        var token = _authenticationService.GenerateAdminToken(employee);

        var tokenModel = new TokenModel { Token = token };

        return Ok(tokenModel);
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<EmployeeModel>> RegisterEmployee(EmployeeRegisterModel employeeRegisterModel)
    {
        var employee = await _employeeService.RegisterAsync(employeeRegisterModel);

        return Ok(employee);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeModel>> UpdateClient(EmployeeUpdateModel employeeUpdateModel)
    {
        var employee = await _employeeService.UpdateAsync(employeeUpdateModel);

        return Ok(employee);
    }

    [HttpDelete]
    public async Task<ActionResult<EmployeeModel>> RemoveClient()
    {
        await _employeeService.RemoveAsync();

        return Ok();
    }
}
