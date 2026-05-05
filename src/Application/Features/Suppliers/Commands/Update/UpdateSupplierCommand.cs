using MediatR;
using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Commands.Update;

public class UpdateSupplierCommand : IRequest<bool>
{
    public int Id { get; }
    public SupplierDto Supplier { get; }
    public UpdateSupplierCommand(int id, SupplierDto supplier)
    {
        Id = id;
        Supplier = supplier;
    }
}