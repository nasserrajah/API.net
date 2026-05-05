using MediatR;

namespace core_first.Application.Features.Customers.Commands.Delete;

public class DeleteCustomerCommand : IRequest<bool>
{
    public int Id { get; set; }
}