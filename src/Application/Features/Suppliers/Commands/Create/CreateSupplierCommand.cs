using MediatR;
using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Commands.Create;

public class CreateSupplierCommand : IRequest<SupplierDto>
{
    public SupplierDto Supplier { get; }
    public CreateSupplierCommand(SupplierDto supplier) => Supplier = supplier;
}