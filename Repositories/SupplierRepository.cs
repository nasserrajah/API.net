using Microsoft.EntityFrameworkCore;
using core_first.API.Data;
using core_first.API.Models;

namespace core_first.API.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Supplier>> GetSuppliersWithTransactionsAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Transactions)
                .ToListAsync();
        }
    }
}