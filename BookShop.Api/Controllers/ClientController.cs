using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
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

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ClientRegisterVm>> RegisterClient(ClientRegisterVm clientModel)
    {
        await _clientService.RegisterAsync(clientModel);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveClient(long clientId)
    {
        await _clientService.RemoveAsync(clientId);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<ClientUpdateVm>> UpdateClient(ClientUpdateVm clientModel)
    {
        await _clientService.UpdateAsync(clientModel);

        return Ok();
    }
}
