using PersianInvoicing.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PersianInvoicing.Views
{
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private async void ProductsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductsViewModel viewModel)
            {
                await viewModel.LoadProductsCommand.ExecuteAsync(null);
            }
        }
    }
}