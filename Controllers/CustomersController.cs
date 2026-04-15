using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.API.Models;
using core_first.API.Repositories;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] string? phone,
            [FromQuery] string? email)
        {
            var customers = await _customerRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
                customers = customers.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(phone))
                customers = customers.Where(c => c.Phone.Contains(phone));
            if (!string.IsNullOrWhiteSpace(email))
                customers = customers.Where(c => c.Email.Contains(email));

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Create(Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            var created = await _customerRepository.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

      [HttpPut("{id}")]
[Authorize(Roles = "Admin,Accountant")]
public async Task<IActionResult> Update(int id, [FromBody] Customer updatedCustomer)
{
    var existing = await _customerRepository.GetByIdAsync(id);
    if (existing == null) return NotFound();
    
    // تحديث الحقول المسموح بها فقط
    existing.Name = updatedCustomer.Name;
    existing.Phone = updatedCustomer.Phone;
    existing.Email = updatedCustomer.Email;
    existing.Address = updatedCustomer.Address;
    // لا نغير CreatedAt أو Id
    
    await _customerRepository.UpdateAsync(existing);
    return NoContent();
}
    }
}