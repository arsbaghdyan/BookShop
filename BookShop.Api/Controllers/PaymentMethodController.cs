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

    [HttpPost("Add_card")]
    public async Task<ActionResult<PaymentMethodModel>> AddPaymentMethod(PaymentMethodAddModel paymentMethodAddModel)
    {
        var paymentMethod = await _paymentMethodService.AddAsync(paymentMethodAddModel);

        return Ok(paymentMethod);
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodModel>>> GetAllPaymentMethods()
    {
        var paymentMethods = await _paymentMethodService.GetAllAsync();

        return Ok(paymentMethods);
    }

    [HttpDelete]
    public async Task<IActionResult> RemovePaymentMethod(long paymentMethodId)
    {
        await _paymentMethodService.RemoveAsync(paymentMethodId);

        return Ok();
    }
}
