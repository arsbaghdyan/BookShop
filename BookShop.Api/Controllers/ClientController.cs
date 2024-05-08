﻿using AutoMapper;
using BookShop.Api.Models.ClientModels;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IMapper _mapper;

    public ClientController(IClientService clientService, IMapper mapper)
    {
        _clientService = clientService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ClientEntity>> RegisterClient(ClientRegisterModel clientModel)
    {
        var client = _mapper.Map<ClientEntity>(clientModel);
        await _clientService.RegisterAsync(client);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<ClientEntity>> RemoveClient(ClientRemoveModel clientRemoveModel)
    {
        var client = _mapper.Map<ClientEntity>(clientRemoveModel);
        await _clientService.RemoveAsync(client);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<ClientEntity>> UpdateClient(ClientUpdateModel clientModel)
    {
        var client = _mapper.Map<ClientEntity>(clientModel);
        await _clientService.UpdateAsync(client);
        return Ok();
    }
}
