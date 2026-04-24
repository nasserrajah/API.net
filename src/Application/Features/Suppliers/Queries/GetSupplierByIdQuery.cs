using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Queries;

public class GetSupplierByIdQuery : IGetSupplierByIdQuery
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierByIdQuery(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto?> ExecuteAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return null;
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address
        };
    }
}