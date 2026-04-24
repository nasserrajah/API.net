using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Features.Customers.Interfaces;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IGetAllCustomersQuery _getAllCustomersQuery;
    private readonly IGetCustomerByIdQuery _getCustomerByIdQuery;
    private readonly ICreateCustomerCommand _createCustomerCommand;
    private readonly IUpdateCustomerCommand _updateCustomerCommand;
    private readonly IDeleteCustomerCommand _deleteCustomerCommand;

    public CustomersController(
        IGetAllCustomersQuery getAllCustomersQuery,
        IGetCustomerByIdQuery getCustomerByIdQuery,
        ICreateCustomerCommand createCustomerCommand,
        IUpdateCustomerCommand updateCustomerCommand,
        IDeleteCustomerCommand deleteCustomerCommand)
    {
        _getAllCustomersQuery = getAllCustomersQuery;
        _getCustomerByIdQuery = getCustomerByIdQuery;
        _createCustomerCommand = createCustomerCommand;
        _updateCustomerCommand = updateCustomerCommand;
        _deleteCustomerCommand = deleteCustomerCommand;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? phone, [FromQuery] string? email)
        => Ok(await _getAllCustomersQuery.ExecuteAsync(name, phone, email));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _getCustomerByIdQuery.ExecuteAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Create(CustomerDto customerDto)
    {
        var created = await _createCustomerCommand.ExecuteAsync(customerDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Update(int id, CustomerDto customerDto)
    {
        var success = await _updateCustomerCommand.ExecuteAsync(id, customerDto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _deleteCustomerCommand.ExecuteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}