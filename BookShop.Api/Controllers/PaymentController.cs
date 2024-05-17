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

    [HttpPost]
    public async Task<ActionResult<PaymentModel>> ApprovePayment(PaymentAddModel paymentAddModel)
    {
        var payment = await _paymentService.ApprovePayment(paymentAddModel);

        return Ok(payment);
    }
}
