using Microsoft.EntityFrameworkCore;
using core_first.API.Data;
using core_first.API.Models;

namespace core_first.API.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Customer>> GetCustomersWithTransactionsAsync()
        {
            return await _context.Customers
                .Include(c => c.Transactions)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerWithTransactionsAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}