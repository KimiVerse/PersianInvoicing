using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersianInvoicing.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseContext _context;

        public ReportService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var invoices = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.IssueDate.Date >= startDate.Date && i.IssueDate.Date <= endDate.Date)
                .ToListAsync();

            var totalSales = invoices.Sum(i => i.FinalPrice);
            var totalInvoices = invoices.Count;
            var averageSale = totalInvoices > 0 ? totalSales / totalInvoices : 0;

            return $@"گزارش فروش از {startDate:yyyy/MM/dd} تا {endDate:yyyy/MM/dd}
---------------------------------
تعداد کل فاکتورها: {totalInvoices}
مجموع فروش: {totalSales:N0} ریال
میانگین فروش هر فاکتور: {averageSale:N0} ریال";
        }
    }
}