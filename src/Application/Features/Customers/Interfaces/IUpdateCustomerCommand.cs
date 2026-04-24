using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Interfaces;

public interface IUpdateCustomerCommand
{
    Task<bool> ExecuteAsync(int id, CustomerDto customerDto);
}