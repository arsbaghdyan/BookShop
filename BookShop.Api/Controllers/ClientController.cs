﻿using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.ClientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class ClientController : BaseClientAuthorizedController
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ClientModel>> RegisterClient(ClientRegisterModel clientModel)
    {
        var client = await _clientService.RegisterAsync(clientModel);

        return Ok(client);
    }

    [HttpPut]
    public async Task<ActionResult<ClientModel>> UpdateClient(ClientUpdateModel clientModel)
    {
        var client = await _clientService.UpdateAsync(clientModel);

        return Ok(client);
    }

    [HttpDelete]
    public async Task<ActionResult<ClientModel>> RemoveClient()
    {
        await _clientService.RemoveAsync();

        return Ok();
    }
}
