using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using core_first.API.Models;
using core_first.API.Services;
using core_first.API.Repositories;

namespace core_first.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Accountant")]
    public class ReportsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly PdfService _pdfService;

        public ReportsController(ITransactionRepository transactionRepository, ICustomerRepository customerRepository, PdfService pdfService)
        {
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _pdfService = pdfService;
        }

        // ------------------- Profit & Loss -------------------
        [HttpGet("profit-loss/excel")]
        public async Task<IActionResult> GetProfitLossExcel([FromQuery] DateTime from, DateTime to)
        {
            var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(fromUtc, toUtc);

            var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var netProfit = totalIncome - totalExpense;

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("ProfitLoss");
            sheet.Cells[1, 1].Value = "البيان";
            sheet.Cells[1, 2].Value = "القيمة";
            sheet.Cells[2, 1].Value = "الفترة من";
            sheet.Cells[2, 2].Value = from.ToString("yyyy-MM-dd");
            sheet.Cells[3, 1].Value = "إلى";
            sheet.Cells[3, 2].Value = to.ToString("yyyy-MM-dd");
            sheet.Cells[5, 1].Value = "إجمالي الإيرادات";
            sheet.Cells[5, 2].Value = totalIncome;
            sheet.Cells[6, 1].Value = "إجمالي المصروفات";
            sheet.Cells[6, 2].Value = totalExpense;
            sheet.Cells[7, 1].Value = "صافي الربح";
            sheet.Cells[7, 2].Value = netProfit;

            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProfitLoss.xlsx");
        }

        [HttpGet("profit-loss/pdf")]
        public async Task<IActionResult> GetProfitLossPdf([FromQuery] DateTime from, DateTime to)
        {
            try
            {
                var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);
                var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(fromUtc, toUtc);

                var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
                var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
                var netProfit = totalIncome - totalExpense;

                var report = new ProfitLossReport
                {
                    StartDate = fromUtc,
                    EndDate = toUtc,
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    NetProfit = netProfit
                };
                var pdfBytes = _pdfService.GenerateProfitLossPdf(report);
                return File(pdfBytes, "application/pdf", "ProfitLoss.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        // ------------------- Transactions by Date -------------------
        [HttpGet("transactions/excel")]
        public async Task<IActionResult> GetTransactionsExcel([FromQuery] DateTime from, DateTime to)
        {
            var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(fromUtc, toUtc);

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Transactions");
            sheet.Cells[1, 1].Value = "التاريخ";
            sheet.Cells[1, 2].Value = "النوع";
            sheet.Cells[1, 3].Value = "المبلغ";
            sheet.Cells[1, 4].Value = "الوصف";
            int row = 2;
            foreach (var t in transactions)
            {
                sheet.Cells[row, 1].Value = t.Date.ToString("yyyy-MM-dd");
                sheet.Cells[row, 2].Value = t.Type.ToString();
                sheet.Cells[row, 3].Value = t.Amount;
                sheet.Cells[row, 4].Value = t.Description;
                row++;
            }
            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
        }

        // ------------------- Customers Report -------------------
        [HttpGet("customers/excel")]
        public async Task<IActionResult> GetCustomersExcel()
        {
            var customers = await _customerRepository.GetCustomersWithTransactionsAsync();

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Customers");
            sheet.Cells[1, 1].Value = "الاسم";
            sheet.Cells[1, 2].Value = "الهاتف";
            sheet.Cells[1, 3].Value = "البريد الإلكتروني";
            sheet.Cells[1, 4].Value = "العنوان";
            sheet.Cells[1, 5].Value = "تاريخ التسجيل";
            sheet.Cells[1, 6].Value = "إجمالي المشتريات";
            sheet.Cells[1, 7].Value = "عدد المعاملات";

            int row = 2;
            foreach (var c in customers)
            {
                var totalAmount = c.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
                sheet.Cells[row, 1].Value = c.Name;
                sheet.Cells[row, 2].Value = c.Phone;
                sheet.Cells[row, 3].Value = c.Email;
                sheet.Cells[row, 4].Value = c.Address;
                sheet.Cells[row, 5].Value = c.CreatedAt.ToString("yyyy-MM-dd");
                sheet.Cells[row, 6].Value = totalAmount;
                sheet.Cells[row, 7].Value = c.Transactions.Count;
                row++;
            }

            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
        }

        [HttpGet("customers/pdf")]
        public async Task<IActionResult> GetCustomersPdf()
        {
            var customers = await _customerRepository.GetCustomersWithTransactionsAsync();
var pdfBytes = _pdfService.GenerateCustomersPdf(customers.ToList());
            return File(pdfBytes, "application/pdf", "Customers.pdf");
        }
    }
}