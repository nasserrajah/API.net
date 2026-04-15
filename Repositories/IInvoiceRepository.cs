using core_first.API.Models;

namespace core_first.API.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice?> GetInvoiceWithItemsAsync(int id);
        Task<IEnumerable<Invoice>> GetAllInvoicesWithItemsAsync();
    }
}