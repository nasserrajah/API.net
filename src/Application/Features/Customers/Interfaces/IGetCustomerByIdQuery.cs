using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Interfaces;

public interface IGetCustomerByIdQuery
{
    Task<CustomerDto?> ExecuteAsync(int id);
}