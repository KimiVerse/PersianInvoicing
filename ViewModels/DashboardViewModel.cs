using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PersianInvoicing.Data;
using PersianInvoicing.Models;
using PersianInvoicing.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PersianInvoicing.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;
        private readonly IMessageService _messageService;

        [ObservableProperty]
        private int _todayInvoiceCount;

        [ObservableProperty]
        private decimal _todayTotalSales;

        [ObservableProperty]
        private int _totalProductsCount;

        [ObservableProperty]
        private ObservableCollection<Invoice> _recentInvoices = new();

        [ObservableProperty]
        private bool _isLoading;

        public DashboardViewModel(DatabaseContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }

        [RelayCommand]
        private async Task LoadDashboardDataAsync()
        {
            IsLoading = true;
            try
            {
                var today = DateTime.Today;
                var todayInvoices = await _context.Invoices
                    .AsNoTracking()
                    .Where(i => i.IssueDate.Date == today)
                    .ToListAsync();

                TodayInvoiceCount = todayInvoices.Count;
                TodayTotalSales = todayInvoices.Sum(i => i.FinalPrice);

                TotalProductsCount = await _context.Products.CountAsync();

                var recent = await _context.Invoices
                    .AsNoTracking()
                    .OrderByDescending(i => i.IssueDate)
                    .Take(5)
                    .ToListAsync();

                RecentInvoices.Clear();
                foreach (var invoice in recent)
                {
                    RecentInvoices.Add(invoice);
                }
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"خطا در بارگذاری اطلاعات داشبورد: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}