using core_first.API.Models;

namespace core_first.API.DTOs
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? InvoiceId { get; set; }
        public string? AttachmentBase64 { get; set; } // اختياري
    }
}