using MediatR;

namespace core_first.Application.Features.Suppliers.Commands.Delete;

public class DeleteSupplierCommand : IRequest<bool>
{
    public int Id { get; set; }
}