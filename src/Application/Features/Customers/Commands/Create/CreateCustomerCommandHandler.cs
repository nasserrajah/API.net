using MediatR;
using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;

namespace core_first.Application.Features.Customers.Commands.Create;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Customer;
        var customer = new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _customerRepository.AddAsync(customer);
        return new CustomerDto
        {
            Id = created.Id,
            Name = created.Name,
            Phone = created.Phone,
            Email = created.Email,
            Address = created.Address
        };
    }
}