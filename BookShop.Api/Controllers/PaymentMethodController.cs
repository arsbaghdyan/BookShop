using BookShop.Api.Controllers.Base;
using BookShop.Data.Models;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class PaymentMethodController : BaseClientAuthorizedController
{
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BankCardInfo>>> GetAllPaymentMethods()
    {
        var paymentMethods = await _paymentMethodService.GetAllAsync();

        return Ok(paymentMethods);
    }

    [HttpPost("card")]
    public async Task<ActionResult<BankCardInfo>> AddPaymentMethod(CardDetails cardDetails)
    {
        var paymentMethod = await _paymentMethodService.AddCardAsync(cardDetails);

        return Ok(paymentMethod);
    }

    [HttpDelete]
    public async Task<IActionResult> RemovePaymentMethod(long paymentMethodId)
    {
        await _paymentMethodService.RemoveAsync(paymentMethodId);

        return Ok();
    }
}
