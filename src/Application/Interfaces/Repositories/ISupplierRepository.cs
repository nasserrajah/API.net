using core_first.Domain.Entities;

namespace core_first.Application.Interfaces.Repositories;

public interface ISupplierRepository : IGenericRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetFilteredAsync(string? name, string? phone, string? email);
}