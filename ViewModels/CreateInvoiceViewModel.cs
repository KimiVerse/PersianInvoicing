using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Data;
using PersianInvoicing.Models;
using PersianInvoicing.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PersianInvoicing.ViewModels
{
    public partial class CreateInvoiceViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;
        private readonly IMessageService _messageService;
        private readonly IPrintService _printService;

        [ObservableProperty]
        private string _customerName = string.Empty;

        [ObservableProperty]
        private string _invoiceNumber = string.Empty;

        [ObservableProperty]
        private DateTime _issueDate = DateTime.Now;

        [ObservableProperty]
        private ObservableCollection<InvoiceItem> _currentInvoiceItems = new();

        [ObservableProperty]
        private ObservableCollection<Product> _availableProducts = new();

        [ObservableProperty]
        private Product? _selectedProduct;

        [ObservableProperty]
        private int _quantity = 1;

        [ObservableProperty]
        private decimal _subTotal;

        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private decimal _finalTotal;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isSaving;

        [ObservableProperty]
        private string _invoiceHeaderStyle = "Modern";

        [ObservableProperty]
        private ObservableCollection<string> _availableHeaderStyles = new()
        {
            "Modern", "Classic", "Elegant", "Professional"
        };

        public CreateInvoiceViewModel(DatabaseContext context, IMessageService messageService, IPrintService printService)
        {
            _context = context;
            _messageService = messageService;
            _printService = printService;
            GenerateInvoiceNumber();
            _ = LoadProductsAsync();
        }

        [RelayCommand]
        private void AddInvoiceItem()
        {
            if (SelectedProduct == null)
            {
                _messageService.ShowWarning("لطفاً یک کالا انتخاب کنید.");
                return;
            }
            if (Quantity <= 0)
            {
                _messageService.ShowWarning("تعداد باید بزرگتر از صفر باشد.");
                return;
            }
            if (Quantity > SelectedProduct.StockQuantity)
            {
                _messageService.ShowWarning($"موجودی کافی نیست. موجودی فعلی: {SelectedProduct.StockQuantity}");
                return;
            }

            var existingItem = CurrentInvoiceItems.FirstOrDefault(i => i.ProductId == SelectedProduct.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += Quantity;
                existingItem.RowTotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                var item = new InvoiceItem
                {
                    Product = SelectedProduct,
                    ProductId = SelectedProduct.Id,
                    Quantity = Quantity,
                    UnitPrice = SelectedProduct.SalePrice,
                    RowTotal = Quantity * SelectedProduct.SalePrice
                };
                CurrentInvoiceItems.Add(item);
            }

            CalculateTotals();
            SelectedProduct = null;
            Quantity = 1;
        }

        [RelayCommand]
        private void RemoveInvoiceItem(InvoiceItem? item)
        {
            if (item != null)
            {
                CurrentInvoiceItems.Remove(item);
                CalculateTotals();
            }
        }

        [RelayCommand]
        private async Task SaveInvoiceAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                _messageService.ShowWarning("لطفاً نام مشتری را وارد کنید.");
                return;
            }
            if (!CurrentInvoiceItems.Any())
            {
                _messageService.ShowWarning("لطفاً حداقل یک کالا به فاکتور اضافه کنید.");
                return;
            }

            IsSaving = true;
            try
            {
                var invoice = new Invoice
                {
                    InvoiceNumber = InvoiceNumber,
                    CustomerName = CustomerName,
                    IssueDate = IssueDate,
                    TotalPrice = SubTotal,
                    Discount = Discount,
                    FinalPrice = FinalTotal,
                    Items = new List<InvoiceItem>() // EF will handle this
                };

                // Add invoice and items to context
                _context.Invoices.Add(invoice);
                foreach (var item in CurrentInvoiceItems)
                {
                    var productInDb = await _context.Products.FindAsync(item.ProductId);
                    if (productInDb != null)
                    {
                        productInDb.StockQuantity -= item.Quantity;
                        invoice.Items.Add(new InvoiceItem
                        {
                            Invoice = invoice,
                            Product = productInDb,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            RowTotal = item.RowTotal
                        });
                    }
                }

                await _context.SaveChangesAsync();

                _messageService.ShowSuccess($"فاکتور شماره {InvoiceNumber} با موفقیت ثبت شد.");

                if (_messageService.ShowConfirmation("آیا می‌خواهید فاکتور را چاپ کنید؟"))
                {
                    await _printService.PrintInvoiceAsync(invoice);
                }

                ClearForm();
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در ثبت فاکتور: {ex.Message}");
            }
            finally
            {
                IsSaving = false;
            }
        }

        private async Task LoadProductsAsync()
        {
            IsLoading = true;
            try
            {
                var products = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.StockQuantity > 0)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();

                AvailableProducts.Clear();
                foreach (var product in products)
                {
                    AvailableProducts.Add(product);
                }
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در بارگذاری کالاها: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateTotals()
        {
            SubTotal = CurrentInvoiceItems.Sum(i => i.RowTotal);
            FinalTotal = SubTotal - Discount;
        }

        partial void OnDiscountChanged(decimal value) => CalculateTotals();

        private void GenerateInvoiceNumber()
        {
            InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }

        private void ClearForm()
        {
            CustomerName = string.Empty;
            CurrentInvoiceItems.Clear();
            Discount = 0;
            GenerateInvoiceNumber();
            CalculateTotals();
        }
    }
}