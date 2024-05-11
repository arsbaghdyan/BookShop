using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentMethodAddVm>> AddPaymentMethod(PaymentMethodAddVm paymentMethodAddModel)
    {
        await _paymentMethodService.AddAsync(paymentMethodAddModel);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodGetVm>>> GetAllPaymentMethods(long clientId)
    {
        var paymentMethods = await _paymentMethodService.GetAllAsync(clientId);

        return Ok(paymentMethods);
    }

    [HttpDelete]
    public async Task<ActionResult> RemovePaymentMethod(long clientId)
    {
        await _paymentMethodService.RemoveAsync(clientId);

        return Ok();
    }
}
