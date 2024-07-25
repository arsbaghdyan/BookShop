using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.PaymentModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class PaymentController : BaseClientAuthorizedController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("{paymentId}")]
    public async Task<ActionResult<PaymentModel>> GetPaymentById(long paymentId)
    {
        var payment = await _paymentService.GetByIdAsync(paymentId);

        return Ok(payment);
    }
}
