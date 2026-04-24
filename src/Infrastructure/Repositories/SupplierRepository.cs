using Microsoft.EntityFrameworkCore;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;
using core_first.Infrastructure.Persistence;

namespace core_first.Infrastructure.Repositories;

public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Supplier>> GetFilteredAsync(string? name, string? phone, string? email)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(s => EF.Functions.ILike(s.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(s => s.Phone.Contains(phone));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(s => EF.Functions.ILike(s.Email, $"%{email}%"));

        return await query.ToListAsync();
    }
}