using System.ComponentModel.DataAnnotations;

namespace PersianInvoicing.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public int StockQuantity { get; set; }

        public string Unit { get; set; } = "عدد";
    }
}