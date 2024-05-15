using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("Registration")]
    [AllowAnonymous]
    public async Task<ActionResult<ClientModel>> RegisterClient(ClientRegisterModel clientModel)
    {
        var client = await _clientService.RegisterAsync(clientModel);

        return Ok(client);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveClient()
    {
        await _clientService.RemoveAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<ClientModel>> UpdateClient(ClientUpdateModel clientModel)
    {
        var client = await _clientService.UpdateAsync(clientModel);

        return Ok(client);
    }
}
