using core_first.API.Models;

namespace core_first.API.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersWithTransactionsAsync();
        Task<Customer?> GetCustomerWithTransactionsAsync(int id);
    }
}