using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersianInvoicing.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalPrice { get; set; }

        // This navigation property will be populated by EF Core
        public virtual List<InvoiceItem> Items { get; set; } = new();
    }
}