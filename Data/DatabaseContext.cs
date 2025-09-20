using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Models;

namespace PersianInvoicing.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PersianInvoicing.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(i => i.Invoice)
                .WithMany(inv => inv.Items)
                .HasForeignKey(i => i.InvoiceId);
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
            // Persian collation for string fields
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entityType.GetProperties())
                {
                    if (prop.ClrType == typeof(string))
                        prop.SetCollation("Persian_100_CI_AI");
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}