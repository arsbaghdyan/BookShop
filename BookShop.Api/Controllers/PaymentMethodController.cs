﻿using AutoMapper;
using BookShop.Data.Entities;
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
    private readonly IMapper _mapper;

    public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper)
    {
        _paymentMethodService = paymentMethodService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentMethodEntity>> AddPaymentMethod(PaymentMethodAddVm paymentMethodAddModel)
    {
        var paymentMethod = _mapper.Map<PaymentMethodEntity>(paymentMethodAddModel);
        await _paymentMethodService.AddAsync(paymentMethod);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodGetVm>>> GetAllPaymentMethods(long clientId)
    {
        var paymentMethods = await _paymentMethodService.GetAllAsync(clientId);
        var paymentMethodList = new List<PaymentMethodGetVm>();
        foreach (var paymentMethod in paymentMethods)
        {
            var paymentMethodOutput = _mapper.Map<PaymentMethodGetVm>(paymentMethod);
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
