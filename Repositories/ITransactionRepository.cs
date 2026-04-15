using core_first.API.Models;

namespace core_first.API.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsWithDetailsAsync();
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime from, DateTime to);
    }
}