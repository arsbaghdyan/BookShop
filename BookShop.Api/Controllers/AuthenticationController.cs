﻿using BookShop.Api.Models.ClientModels;
using BookShop.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authenticationService;

    public AuthenticationController(ICustomAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(ClientLoginModel model)
    {
        var clientEntity = await _authenticationService.AuthenticateAsync(model.Email, model.Password);
        if (clientEntity != null)
        {
            var token = _authenticationService.GenerateToken(clientEntity);

            return Ok(new { token });
        }
        else
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }
}
