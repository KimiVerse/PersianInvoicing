using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersianInvoicing.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "شماره فاکتور")]
        public string InvoiceNumber { get; set; } = Guid.NewGuid().ToString("N")[..8];
        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; } = string.Empty;
        [Display(Name = "توضیحات")]
        public string Description { get; set; } = string.Empty;
        public virtual List<InvoiceItem> Items { get; set; } = new();
        [Display(Name = "جمع کل")]
        public decimal TotalAmount => Items.Sum(i => i.Amount);
    }

    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; } = null!;
        [Display(Name = "محصول")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        [Display(Name = "تعداد")]
        public int Quantity { get; set; }
        [Display(Name = "قیمت واحد")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "مبلغ")]
        public decimal Amount => Quantity * UnitPrice;
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "نام محصول")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
    }
}