using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Interfaces;

public interface IGetAllSuppliersQuery
{
    Task<IEnumerable<SupplierDto>> ExecuteAsync(string? name, string? phone, string? email);
}