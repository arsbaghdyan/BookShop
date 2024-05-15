using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
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
        var client = await _clientService.RegisterAsync(clientModel);

        return Ok(client);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<ClientEntity>> RemoveClient()
    {
        await _clientService.RemoveAsync();

        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ClientEntity>> UpdateClient(ClientUpdateModel clientModel)
    {
        var client = await _clientService.UpdateAsync(clientModel);

        return Ok(client);
    }
}
