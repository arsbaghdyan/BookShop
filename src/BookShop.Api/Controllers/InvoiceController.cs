using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class InvoiceController : BaseClientAuthorizedController
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("{invoiceId}")]
    public async Task<ActionResult<InvoiceModel>> GetInvoiceById(long invoiceId)
    {
        var invoice = await _invoiceService.GetByIdAsync(invoiceId);

        return Ok(invoice);
    }
}
