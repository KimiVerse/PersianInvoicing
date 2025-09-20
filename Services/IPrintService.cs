using PersianInvoicing.Models;

namespace PersianInvoicing.Services
{
    public interface IPrintService
    {
        void PrintInvoice(Invoice invoice);
    }
}