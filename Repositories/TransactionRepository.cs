using Microsoft.EntityFrameworkCore;
using core_first.API.Data;
using core_first.API.Models;

namespace core_first.API.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Transaction>> GetTransactionsWithDetailsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Supplier)
                .Include(t => t.Invoice)
                .Include(t => t.User)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Transactions
                .Where(t => t.Date >= from && t.Date <= to)
                .Include(t => t.Customer)
                .Include(t => t.Supplier)
                .ToListAsync();
        }
    }
}