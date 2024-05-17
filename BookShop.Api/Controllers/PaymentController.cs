using BookShop.Services.Abstractions;
using BookShop.Services.Models.PaymentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("Confirm_payment")]
    public async Task<ActionResult<PaymentModel>> ConfirmPayment(PaymentAddModel paymentAddModel)
    {
        var payment = await _paymentService.ConfirmPayment(paymentAddModel);

        return Ok(payment);
    }
}
