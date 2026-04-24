using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Commands;

public class UpdateSupplierCommand : IUpdateSupplierCommand
{
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierCommand(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<bool> ExecuteAsync(int id, SupplierDto supplierDto)
    {
        var existing = await _supplierRepository.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Name = supplierDto.Name;
        existing.Phone = supplierDto.Phone;
        existing.Email = supplierDto.Email;
        existing.Address = supplierDto.Address;
        await _supplierRepository.UpdateAsync(existing);
        return true;
    }
}