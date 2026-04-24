using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Customers.Queries;

public class GetAllCustomersQuery : IGetAllCustomersQuery
{
    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomersQuery(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> ExecuteAsync(string? name, string? phone, string? email)
    {
        var customers = await _customerRepository.GetFilteredAsync(name, phone, email);
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email,
            Address = c.Address
        });
    }
}