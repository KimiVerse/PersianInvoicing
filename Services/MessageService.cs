using System.Windows;

namespace PersianInvoicing.Services
{
    public class MessageService : IMessageService
    {
        public void ShowSuccess(string message)
        {
            MessageBox.Show(message, "موفقیت", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
        }

        public bool ShowConfirmation(string message)
        {
            var result = MessageBox.Show(message, "تأیید", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RtlReading);
            return result == MessageBoxResult.Yes;
        }
    }
}