using AutoMapper;
using BookShop.Api.Models.PaymentMethodModels;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IMapper _mapper;

    public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper)
    {
        _paymentMethodService = paymentMethodService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentMethodEntity>> AddPaymentMethod(PaymentMethodAddModel paymentMethodAddModel)
    {
        var paymentMethod = _mapper.Map<PaymentMethodEntity>(paymentMethodAddModel);
        await _paymentMethodService.AddAsync(paymentMethod);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodGetModel>>> GetAllPaymentMethods(long clientId)
    {
        var paymentMethods = await _paymentMethodService.GetAllAsync(clientId);
        var paymentMethodList = new List<PaymentMethodGetModel>();
        foreach (var paymentMethod in paymentMethods)
        {
            var paymentMethodOutput = _mapper.Map<PaymentMethodGetModel>(paymentMethod);
            paymentMethodList.Add(paymentMethodOutput);
        }

        return Ok(paymentMethodList);
    }

    [HttpDelete]
    public async Task<ActionResult<PaymentMethodEntity>> RemovePaymentMethod(PaymentMethodDeleteModel paymentMethodDeleteModel)
    {
        var paymentMethod = _mapper.Map<PaymentMethodEntity>(paymentMethodDeleteModel);
        await _paymentMethodService.RemoveAsync(paymentMethod);

        return Ok();
    }
}
