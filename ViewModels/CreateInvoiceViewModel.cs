using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PersianInvoicing.Models;
using PersianInvoicing.Data;
using PersianInvoicing.Services;
using System.Linq;

namespace PersianInvoicing.ViewModels
{
    public class CreateInvoiceViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseContext _db;
        private readonly IPrintService _printService;
        public Invoice CurrentInvoice { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<InvoiceItem> Items { get; set; } = new();

        public CreateInvoiceViewModel(DatabaseContext db, IPrintService printService)
        {
            _db = db;
            _printService = printService;
            LoadProducts();
            SaveCommand = new RelayCommand(SaveInvoice);
            AddItemCommand = new RelayCommand<Product>(AddItem);
            PrintCommand = new RelayCommand(PrintInvoice);
        }

        private void LoadProducts()
        {
            Products = new ObservableCollection<Product>(_db.Products.ToList());
            // Seed sample data if empty
            if (!Products.Any())
            {
                _db.Products.Add(new Product { Name = "محصول نمونه", Price = 10000 });
                _db.SaveChanges();
                LoadProducts();
            }
        }

        private void AddItem(Product selectedProduct)
        {
            if (selectedProduct != null)
            {
                var item = new InvoiceItem { Product = selectedProduct, Quantity = 1, UnitPrice = selectedProduct.Price };
                Items.Add(item);
                CurrentInvoice.Items.Add(item); // For total calc
                OnPropertyChanged(nameof(CurrentInvoice));
            }
        }

        private void SaveInvoice()
        {
            try
            {
                _db.Invoices.Add(CurrentInvoice);
                _db.SaveChanges();
                // Notify user (inject IMessageService)
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void PrintInvoice()
        {
            _printService.PrintInvoice(CurrentInvoice);
        }

        public ICommand SaveCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand PrintCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public RelayCommand(Action<T> execute) => _execute = execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);
    }
}