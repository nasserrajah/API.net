using MediatR;
using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Commands.Update;

public class UpdateCustomerCommand : IRequest<bool>
{
    public int Id { get; }
    public CustomerDto Customer { get; }

    public UpdateCustomerCommand(int id, CustomerDto customer)
    {
        Id = id;
        Customer = customer;
    }
}