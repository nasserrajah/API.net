using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IGetCustomerByIdQuery
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQuery(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto?> ExecuteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address
        };
    }
}