using core_first.API.Models;

namespace core_first.API.DTOs
{
    public class InvoiceItemDto
    {
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class InvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}