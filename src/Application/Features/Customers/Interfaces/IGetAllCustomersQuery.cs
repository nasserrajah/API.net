using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Interfaces;

public interface IGetAllCustomersQuery
{
    Task<IEnumerable<CustomerDto>> ExecuteAsync(string? name, string? phone, string? email);
}