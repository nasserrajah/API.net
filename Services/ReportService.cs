using Microsoft.EntityFrameworkCore;
using core_first.API.Data;
using core_first.API.Models;

namespace core_first.API.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

       public async Task<ProfitLossReport> GetProfitLossAsync(DateTime from, DateTime to)
{
    try
    {
        var transactions = await _context.Transactions
            .Where(t => t.Date >= from && t.Date <= to)
            .ToListAsync();

        var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        return new ProfitLossReport
        {
            StartDate = from,
            EndDate = to,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            NetProfit = totalIncome - totalExpense
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR in GetProfitLossAsync: {ex.Message}");
        throw; // لإعادة الخطأ مع تفاصيله
    }
}

        public async Task<List<Transaction>> GetTransactionsByDateAsync(DateTime from, DateTime to)
        {
            return await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Supplier)
                .Where(t => t.Date >= from && t.Date <= to)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetAllCustomersWithTransactionsAsync()
        {
            return await _context.Customers
                .Include(c => c.Transactions)
                .ToListAsync();
        }
    }

    public class ProfitLossReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetProfit { get; set; }
    }
}