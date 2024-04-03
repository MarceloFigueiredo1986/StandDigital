namespace StandDigital.Services.Toaster;

public interface IAppToastService
{
    void ShowInfo(string message,int timer = 5);
    void ShowSuccess(string message, int timer = 5);
    void ShowWarning(string message, int timer = 5);
    void ShowError(string message, int timer = 5);
}
