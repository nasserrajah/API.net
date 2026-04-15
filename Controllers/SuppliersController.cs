using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.API.Models;
using core_first.API.Repositories;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] string? phone,
            [FromQuery] string? email)
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            var query = suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(s => s.Phone.Contains(phone));
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.Email.Contains(email));

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.UtcNow;
            var created = await _supplierRepository.AddAsync(supplier);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Update(int id, Supplier supplier)
        {
            if (id != supplier.Id) return BadRequest();
            await _supplierRepository.UpdateAsync(supplier);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            await _supplierRepository.DeleteAsync(supplier);
            return NoContent();
        }
    }
}