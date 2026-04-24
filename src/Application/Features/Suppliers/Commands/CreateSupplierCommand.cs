using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;

namespace core_first.Application.Features.Suppliers.Commands;

public class CreateSupplierCommand : ICreateSupplierCommand
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierCommand(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> ExecuteAsync(SupplierDto supplierDto)
    {
        var supplier = new Supplier
        {
            Name = supplierDto.Name,
            Phone = supplierDto.Phone,
            Email = supplierDto.Email,
            Address = supplierDto.Address,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _supplierRepository.AddAsync(supplier);
        supplierDto.Id = created.Id;
        return supplierDto;
    }
}