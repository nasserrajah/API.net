using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Queries;

public class GetAllSuppliersQuery : IGetAllSuppliersQuery
{
    private readonly ISupplierRepository _supplierRepository;

    public GetAllSuppliersQuery(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<SupplierDto>> ExecuteAsync(string? name, string? phone, string? email)
    {
        var suppliers = await _supplierRepository.GetFilteredAsync(name, phone, email);
        return suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address
        });
    }
}