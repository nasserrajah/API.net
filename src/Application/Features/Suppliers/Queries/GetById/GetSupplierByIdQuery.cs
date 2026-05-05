using MediatR;
using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Queries.GetById;

public class GetSupplierByIdQuery : IRequest<SupplierDto?>
{
    public int Id { get; set; }
}