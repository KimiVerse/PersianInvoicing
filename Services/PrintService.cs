using PersianInvoicing.Models;
using PersianInvoicing.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PersianInvoicing.Services
{
    public class PrintService : IPrintService
    {
        public async Task PrintInvoiceAsync(Invoice invoice)
        {
            // The print dialog must be shown on the UI thread.
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    var printView = new PrintableView { DataContext = invoice };

                    // Set its size to the printable area before printing
                    printView.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                    printView.Arrange(new Rect(new Point(0, 0), printView.DesiredSize));
                    printView.UpdateLayout();

                    printDialog.PrintVisual(printView, $"فاکتور شماره {invoice.InvoiceNumber}");
                }
            });
        }
    }
}