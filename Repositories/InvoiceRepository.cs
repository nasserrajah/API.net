using Microsoft.EntityFrameworkCore;
using core_first.API.Data;
using core_first.API.Models;

namespace core_first.API.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context) : base(context) { }

        public async Task<Invoice?> GetInvoiceWithItemsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Supplier)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesWithItemsAsync()
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Supplier)
                .Include(i => i.Items)
                .ToListAsync();
        }
    }
}