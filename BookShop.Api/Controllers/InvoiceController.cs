using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<InvoiceModel>>> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();

        return Ok(invoices);
    }

    [HttpGet("{paymentId}")]
    public async Task<ActionResult<InvoiceModel>> GetById(long paymentId)
    {
        var invoice = await _invoiceService.GetByIdAsync(paymentId);

        return Ok(invoice);
    }

    [HttpDelete]
    public async Task<IActionResult> Clear()
    {
        await _invoiceService.ClearAsync();

        return Ok();
    }
}
