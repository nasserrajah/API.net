using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;

namespace core_first.Application.Features.Customers.Commands;

public class CreateCustomerCommand : ICreateCustomerCommand
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommand(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> ExecuteAsync(CustomerDto customerDto)
    {
        var customer = new Customer
        {
            Name = customerDto.Name,
            Phone = customerDto.Phone,
            Email = customerDto.Email,
            Address = customerDto.Address,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _customerRepository.AddAsync(customer);
        customerDto.Id = created.Id;
        return customerDto;
    }
}