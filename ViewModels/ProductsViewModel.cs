using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Data;
using PersianInvoicing.Models;
using PersianInvoicing.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PersianInvoicing.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;
        private readonly IMessageService _messageService;

        [ObservableProperty]
        private ObservableCollection<Product> _productsList = new();

        [ObservableProperty]
        private Product? _selectedProduct;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _productCode = string.Empty;

        [ObservableProperty]
        private string _productName = string.Empty;

        [ObservableProperty]
        private decimal _purchasePrice;

        [ObservableProperty]
        private decimal _salePrice;

        [ObservableProperty]
        private int _stockQuantity;

        [ObservableProperty]
        private string _unit = "عدد";

        [ObservableProperty]
        private bool _isLoading;

        public ProductsViewModel(DatabaseContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
            _ = LoadProductsAsync(); // Load products on initialization
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductCode) || string.IsNullOrWhiteSpace(ProductName))
            {
                _messageService.ShowWarning("لطفاً کد کالا و نام کالا را وارد کنید.");
                return;
            }

            try
            {
                var isCodeDuplicate = await _context.Products.AnyAsync(p => p.ProductCode == ProductCode);
                if (isCodeDuplicate)
                {
                    _messageService.ShowWarning("کد کالای وارد شده قبلاً استفاده شده است.");
                    return;
                }

                var product = new Product
                {
                    ProductCode = ProductCode,
                    ProductName = ProductName,
                    PurchasePrice = PurchasePrice,
                    SalePrice = SalePrice,
                    StockQuantity = StockQuantity,
                    Unit = Unit
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _messageService.ShowSuccess("کالا با موفقیت اضافه شد.");
                ClearForm();
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در افزودن کالا: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task UpdateProductAsync()
        {
            if (SelectedProduct == null)
            {
                _messageService.ShowWarning("لطفاً یک کالا برای ویرایش انتخاب کنید.");
                return;
            }

            try
            {
                var productToUpdate = await _context.Products.FindAsync(SelectedProduct.Id);
                if (productToUpdate != null)
                {
                    productToUpdate.ProductCode = ProductCode;
                    productToUpdate.ProductName = ProductName;
                    productToUpdate.PurchasePrice = PurchasePrice;
                    productToUpdate.SalePrice = SalePrice;
                    productToUpdate.StockQuantity = StockQuantity;
                    productToUpdate.Unit = Unit;

                    await _context.SaveChangesAsync();
                    _messageService.ShowSuccess("کالا با موفقیت ویرایش شد.");
                }

                ClearForm();
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در ویرایش کالا: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
            {
                _messageService.ShowWarning("لطفاً یک کالا برای حذف انتخاب کنید.");
                return;
            }

            if (!_messageService.ShowConfirmation("آیا از حذف این کالا اطمینان دارید؟")) return;

            try
            {
                var productToDelete = await _context.Products.FindAsync(SelectedProduct.Id);
                if (productToDelete != null)
                {
                    _context.Products.Remove(productToDelete);
                    await _context.SaveChangesAsync();
                    _messageService.ShowSuccess("کالا با موفقیت حذف شد.");
                }

                ClearForm();
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در حذف کالا: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task LoadProductsAsync()
        {
            IsLoading = true;
            try
            {
                var query = _context.Products.AsNoTracking();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    query = query.Where(p => p.ProductName.Contains(SearchText) || p.ProductCode.Contains(SearchText));
                }

                var products = await query.OrderBy(p => p.ProductName).ToListAsync();

                ProductsList.Clear();
                foreach (var product in products)
                {
                    ProductsList.Add(product);
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

        partial void OnSelectedProductChanged(Product? value)
        {
            if (value != null)
            {
                ProductCode = value.ProductCode;
                ProductName = value.ProductName;
                PurchasePrice = value.PurchasePrice;
                SalePrice = value.SalePrice;
                StockQuantity = value.StockQuantity;
                Unit = value.Unit;
            }
            else
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            ProductCode = string.Empty;
            ProductName = string.Empty;
            PurchasePrice = 0;
            SalePrice = 0;
            StockQuantity = 0;
            Unit = "عدد";
            SelectedProduct = null;
        }
    }
}