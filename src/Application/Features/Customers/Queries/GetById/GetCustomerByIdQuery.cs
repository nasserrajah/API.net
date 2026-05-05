using MediatR;
using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Queries.GetById;

public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public int Id { get; set; }
}