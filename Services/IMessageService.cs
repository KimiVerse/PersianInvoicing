namespace PersianInvoicing.Services
{
    public interface IMessageService
    {
        void ShowSuccess(string message);
        void ShowError(string message);
        void ShowWarning(string message);
        bool ShowConfirmation(string message);
    }
}