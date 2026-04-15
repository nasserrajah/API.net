using System.ComponentModel.DataAnnotations.Schema;

namespace core_first.API.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public class Transaction
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? InvoiceId { get; set; }
        public int UserId { get; set; }  // من سجل العملية
        public string? AttachmentPath { get; set; }

        // Navigation properties
        public Customer? Customer { get; set; }
        public Supplier? Supplier { get; set; }
        public Invoice? Invoice { get; set; }
        public User? User { get; set; }
    }
}