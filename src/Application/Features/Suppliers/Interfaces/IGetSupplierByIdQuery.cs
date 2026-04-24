using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Interfaces;

public interface IGetSupplierByIdQuery
{
    Task<SupplierDto?> ExecuteAsync(int id);
}