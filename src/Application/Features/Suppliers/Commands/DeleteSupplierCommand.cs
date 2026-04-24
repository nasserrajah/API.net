using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Commands;

public class DeleteSupplierCommand : IDeleteSupplierCommand
{
    private readonly ISupplierRepository _supplierRepository;

    public DeleteSupplierCommand(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return false;
        await _supplierRepository.DeleteAsync(supplier);
        return true;
    }
}