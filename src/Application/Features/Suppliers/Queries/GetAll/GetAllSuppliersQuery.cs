using MediatR;
using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Queries.GetAll;

public class GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}