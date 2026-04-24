using core_first.Domain.Entities;

namespace core_first.Application.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<IEnumerable<Customer>> GetFilteredAsync(string? name, string? phone, string? email);
}