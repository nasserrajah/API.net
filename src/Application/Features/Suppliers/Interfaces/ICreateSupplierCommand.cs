using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Interfaces;

public interface ICreateSupplierCommand
{
    Task<SupplierDto> ExecuteAsync(SupplierDto supplierDto);
}