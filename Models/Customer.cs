namespace core_first.API.Models
{
    public class Customer
    {
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}