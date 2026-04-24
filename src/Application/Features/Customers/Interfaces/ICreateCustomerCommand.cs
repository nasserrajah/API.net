using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Interfaces;

public interface ICreateCustomerCommand
{
    Task<CustomerDto> ExecuteAsync(CustomerDto customerDto);
}