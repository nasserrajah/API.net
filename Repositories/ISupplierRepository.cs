using core_first.API.Models;

namespace core_first.API.Repositories
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetSuppliersWithTransactionsAsync();
    }
}