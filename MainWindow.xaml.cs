using Microsoft.Extensions.DependencyInjection;
using PersianInvoicing.ViewModels;
using PersianInvoicing.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PersianInvoicing
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // The TabControl now has x:Name="MainTabControl" in the XAML
            if (MainTabControl == null) return;

            // Assign DataContext for each view inside the tabs
            foreach (TabItem tab in MainTabControl.Items)
            {
                if (tab.Content is FrameworkElement view)
                {
                    if (view is DashboardView)
                    {
                        view.DataContext = _serviceProvider.GetRequiredService<DashboardViewModel>();
                    }
                    else if (view is ProductsView)
                    {
                        view.DataContext = _serviceProvider.GetRequiredService<ProductsViewModel>();
                    }
                    else if (view is CreateInvoiceView)
                    {
                        view.DataContext = _serviceProvider.GetRequiredService<CreateInvoiceViewModel>();
                    }
                }
            }
        }
    }
}