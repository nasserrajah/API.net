namespace core_first.API.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}