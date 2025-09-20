using System;
using System.Threading.Tasks;

namespace PersianInvoicing.Services
{
    public interface IReportService
    {
        Task<string> GenerateSalesReportAsync(DateTime startDate, DateTime endDate);
    }
}