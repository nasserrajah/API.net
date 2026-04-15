using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.API.Models;
using core_first.API.DTOs;
using core_first.API.Services;
using core_first.API.Repositories;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly PdfService _pdfService;

        public InvoicesController(IInvoiceRepository invoiceRepository, PdfService pdfService)
        {
            _invoiceRepository = invoiceRepository;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceRepository.GetAllInvoicesWithItemsAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Create([FromBody] InvoiceDto dto)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                CustomerId = dto.CustomerId,
                SupplierId = dto.SupplierId,
                Status = InvoiceStatus.Unpaid,
                TotalAmount = 0
            };

            foreach (var itemDto in dto.Items)
            {
                invoice.Items.Add(new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                });
            }
            invoice.TotalAmount = invoice.Items.Sum(i => i.Total);

            var created = await _invoiceRepository.AddAsync(invoice);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] InvoiceStatus status)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return NotFound();
            invoice.Status = status;
            await _invoiceRepository.UpdateAsync(invoice);
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> GetInvoicePdf(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);
            if (invoice == null) return NotFound();
            var pdfBytes = _pdfService.GenerateInvoicePdf(invoice);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return NotFound();
            await _invoiceRepository.DeleteAsync(invoice);
            return NoContent();
        }
    }
}