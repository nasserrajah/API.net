namespace core_first.API.Models
{
    public enum InvoiceStatus
    {
        Unpaid,
        Paid
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }

        public Customer? Customer { get; set; }
        public Supplier? Supplier { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}