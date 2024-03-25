namespace StandDigital.Services.Toaster;

public interface IAppToastService
{
    void ShowInfo(string message);
    void ShowSuccess(string message);
    void ShowWarning(string message);
    void ShowError(string message);
}
