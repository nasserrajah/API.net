using MediatR;
using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Queries.GetAll;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}