using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Features.Customers.Queries.GetAll;
using core_first.Application.Features.Customers.Queries.GetById;
using core_first.Application.Features.Customers.Commands.Create;
using core_first.Application.Features.Customers.Commands.Update;
using core_first.Application.Features.Customers.Commands.Delete;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? phone, [FromQuery] string? email)
    {
        var query = new GetAllCustomersQuery { Name = name, Phone = phone, Email = email };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var customer = await _mediator.Send(query);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Create(CustomerDto customerDto)
    {
        var command = new CreateCustomerCommand(customerDto);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Update(int id, CustomerDto customerDto)
    {
        var command = new UpdateCustomerCommand(id, customerDto);
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCustomerCommand { Id = id };
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }
}