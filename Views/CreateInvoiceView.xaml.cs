using System.Windows;
using PersianInvoicing.ViewModels;
using PersianInvoicing.Data;
using PersianInvoicing.Services;
using Microsoft.Extensions.DependencyInjection; // Assume DI setup in App.xaml.cs

namespace PersianInvoicing.Views
{
    public partial class CreateInvoiceView : Window
    {
        public CreateInvoiceView()
        {
            InitializeComponent();
            var serviceProvider = (Application.Current as App)?.ServiceProvider;
            DataContext = serviceProvider.GetRequiredService<CreateInvoiceViewModel>();
        }
    }
}