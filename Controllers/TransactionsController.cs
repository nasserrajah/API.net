using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using core_first.API.Models;
using core_first.API.DTOs;
using core_first.API.Repositories;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWebHostEnvironment _env;

        public TransactionsController(ITransactionRepository transactionRepository, IWebHostEnvironment env)
        {
            _transactionRepository = transactionRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionRepository.GetTransactionsWithDetailsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Create([FromBody] TransactionDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description,
                Date = dto.Date,
                CustomerId = dto.CustomerId,
                SupplierId = dto.SupplierId,
                InvoiceId = dto.InvoiceId,
                UserId = userId
            };

            var created = await _transactionRepository.AddAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionDto dto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            transaction.Amount = dto.Amount;
            transaction.Type = dto.Type;
            transaction.Description = dto.Description;
            transaction.Date = dto.Date;
            transaction.CustomerId = dto.CustomerId;
            transaction.SupplierId = dto.SupplierId;
            transaction.InvoiceId = dto.InvoiceId;

            await _transactionRepository.UpdateAsync(transaction);
            return NoContent();
        }

        [HttpPost("{id}/attachment")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> UploadAttachment(int id, IFormFile file)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            transaction.AttachmentPath = filePath;
            await _transactionRepository.UpdateAsync(transaction);
            return Ok(new { path = filePath });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return NotFound();
            await _transactionRepository.DeleteAsync(transaction);
            return NoContent();
        }
    }
}