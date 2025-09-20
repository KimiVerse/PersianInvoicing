using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersianInvoicing.Data;
using PersianInvoicing.Services;
using PersianInvoicing.ViewModels;
using System.Windows;

namespace PersianInvoicing
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register DbContext
                    services.AddDbContext<DatabaseContext>(options =>
                        options.UseSqlite("Data Source=invoicing.db"));

                    // Register Services
                    services.AddScoped<IReportService, ReportService>();
                    services.AddScoped<IMessageService, MessageService>();
                    services.AddScoped<IPrintService, PrintService>();

                    // Register ViewModels
                    services.AddTransient<DashboardViewModel>();
                    services.AddTransient<ProductsViewModel>();
                    services.AddTransient<CreateInvoiceViewModel>();

                    // Register Main Window
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Initialize database on startup
            using (var scope = _host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await context.Database.EnsureCreatedAsync();
            }

            // Create and show main window through DI
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}