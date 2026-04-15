namespace core_first.API.DTOs
{
    public class ReportParametersDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ReportType { get; set; } // "ProfitLoss", "Customers", "Transactions"
    }
}