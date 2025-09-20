using PersianInvoicing.Models;
using PersianInvoicing.Views;
using System.Windows;

namespace PersianInvoicing.Services
{
    public class PrintService : IPrintService
    {
        public void PrintInvoice(Invoice invoice)
        {
            var printableWindow = new PrintableView { DataContext = invoice };
            printableWindow.ShowDialog(); // Or use PrintDialog for actual printing
        }
    }

    public interface IPrintService
    {
        void PrintInvoice(Invoice invoice);
    }
}