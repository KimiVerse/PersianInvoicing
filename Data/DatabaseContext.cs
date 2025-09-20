using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PersianInvoicing.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ProductCode).IsUnique();
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FinalPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.RowTotal).HasColumnType("decimal(18, 2)");
            });
        }
    }
}