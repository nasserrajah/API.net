using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Interfaces;

public interface IUpdateSupplierCommand
{
    Task<bool> ExecuteAsync(int id, SupplierDto supplierDto);
}