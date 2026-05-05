using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Features.Suppliers.Queries.GetAll;
using core_first.Application.Features.Suppliers.Queries.GetById;
using core_first.Application.Features.Suppliers.Commands.Create;
using core_first.Application.Features.Suppliers.Commands.Update;
using core_first.Application.Features.Suppliers.Commands.Delete;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? phone, [FromQuery] string? email)
    {
        var query = new GetAllSuppliersQuery { Name = name, Phone = phone, Email = email };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetSupplierByIdQuery { Id = id };
        var supplier = await _mediator.Send(query);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Create(SupplierDto supplierDto)
    {
        var command = new CreateSupplierCommand(supplierDto);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Update(int id, SupplierDto supplierDto)
    {
        var command = new UpdateSupplierCommand(id, supplierDto);
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteSupplierCommand { Id = id };
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }
}