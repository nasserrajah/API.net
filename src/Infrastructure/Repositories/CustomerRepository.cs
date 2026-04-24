using Microsoft.EntityFrameworkCore;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;
using core_first.Infrastructure.Persistence;

namespace core_first.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Customer>> GetFilteredAsync(string? name, string? phone, string? email)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(c => c.Phone.Contains(phone));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(c => EF.Functions.ILike(c.Email, $"%{email}%"));

        return await query.ToListAsync();
    }
}