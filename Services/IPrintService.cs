using PersianInvoicing.Models;
using System.Threading.Tasks;

namespace PersianInvoicing.Services
{
    public interface IPrintService
    {
        Task PrintInvoiceAsync(Invoice invoice);
    }
}