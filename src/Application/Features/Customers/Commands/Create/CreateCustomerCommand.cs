using MediatR;
using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public CustomerDto Customer { get; }

    public CreateCustomerCommand(CustomerDto customer) => Customer = customer;
}