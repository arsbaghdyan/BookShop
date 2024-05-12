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
    [HttpPost("Registration")]
    public async Task<ActionResult<ClientRegisterModel>> RegisterClient(ClientRegisterModel clientModel)
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
    public async Task<ActionResult<ClientUpdateModel>> UpdateClient(ClientUpdateModel clientModel)
    {
        await _clientService.UpdateAsync(clientModel);

        return Ok();
    }

}
