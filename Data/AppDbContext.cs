using Microsoft.EntityFrameworkCore;
using core_first.API.Models;

namespace core_first.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // علاقة Transaction مع Customer (اختياري)
    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Customer)
        .WithMany(c => c.Transactions)
        .HasForeignKey(t => t.CustomerId)
        .OnDelete(DeleteBehavior.Restrict);

    // علاقة Transaction مع Supplier
    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Supplier)
        .WithMany(s => s.Transactions)
        .HasForeignKey(t => t.SupplierId)
        .OnDelete(DeleteBehavior.Restrict);

    // علاقة InvoiceItem مع Invoice
    modelBuilder.Entity<InvoiceItem>()
        .HasOne(i => i.Invoice)
        .WithMany(i => i.Items)
        .HasForeignKey(i => i.InvoiceId);

    // علاقات الفاتورة مع العملاء/الموردين (بدون تكرار)
    modelBuilder.Entity<Invoice>()
        .HasOne(i => i.Customer)
        .WithMany()
        .HasForeignKey(i => i.CustomerId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Invoice>()
        .HasOne(i => i.Supplier)
        .WithMany()
        .HasForeignKey(i => i.SupplierId)
        .OnDelete(DeleteBehavior.Restrict);
}
    }
}