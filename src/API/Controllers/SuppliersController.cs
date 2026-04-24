using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Features.Suppliers.Interfaces;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IGetAllSuppliersQuery _getAllSuppliersQuery;
    private readonly IGetSupplierByIdQuery _getSupplierByIdQuery;
    private readonly ICreateSupplierCommand _createSupplierCommand;
    private readonly IUpdateSupplierCommand _updateSupplierCommand;
    private readonly IDeleteSupplierCommand _deleteSupplierCommand;

    public SuppliersController(
        IGetAllSuppliersQuery getAllSuppliersQuery,
        IGetSupplierByIdQuery getSupplierByIdQuery,
        ICreateSupplierCommand createSupplierCommand,
        IUpdateSupplierCommand updateSupplierCommand,
        IDeleteSupplierCommand deleteSupplierCommand)
    {
        _getAllSuppliersQuery = getAllSuppliersQuery;
        _getSupplierByIdQuery = getSupplierByIdQuery;
        _createSupplierCommand = createSupplierCommand;
        _updateSupplierCommand = updateSupplierCommand;
        _deleteSupplierCommand = deleteSupplierCommand;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? phone, [FromQuery] string? email)
        => Ok(await _getAllSuppliersQuery.ExecuteAsync(name, phone, email));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _getSupplierByIdQuery.ExecuteAsync(id);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Create(SupplierDto supplierDto)
    {
        var created = await _createSupplierCommand.ExecuteAsync(supplierDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<IActionResult> Update(int id, SupplierDto supplierDto)
    {
        var success = await _updateSupplierCommand.ExecuteAsync(id, supplierDto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _deleteSupplierCommand.ExecuteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}