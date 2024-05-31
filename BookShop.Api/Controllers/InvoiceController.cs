using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class InvoiceController : BaseAuthorizedController
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<InvoiceModel>> GetInvoiceById(long orderId)
    {
        var invoice = await _invoiceService.GetByIdAsync(orderId);

        return Ok(invoice);
    }
}
