using PersianInvoicing.Models;
using PersianInvoicing.Views;
using System;
using System.Windows;

namespace PersianInvoicing.Services
{
    public class PrintService : IPrintService
    {
        public void PrintInvoice(Invoice invoice)
        {
            try
            {
                var printableWindow = new PrintableView();
                printableWindow.DataContext = invoice;
                printableWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در چاپ: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}