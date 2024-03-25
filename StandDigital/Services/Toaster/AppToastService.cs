using Blazored.Toast.Configuration;
using Blazored.Toast.Services;

namespace StandDigital.Services.Toaster;

public class AppToastService(IToastService toastService) : IAppToastService
{
    private readonly IToastService _toastService = toastService;

    Action<ToastSettings> Action => a => {
        a.Position = ToastPosition.BottomLeft;
        a.Timeout = 5;
        a.ShowProgressBar = true;
    };

    public void ShowError(string message)
        => _toastService.ShowError(message, Action);

    public void ShowInfo(string message)
        => _toastService.ShowInfo(message, Action);

    public void ShowSuccess(string message)
        => _toastService.ShowSuccess(message, Action);

    public void ShowWarning(string message)
        => _toastService.ShowWarning(message, Action);
}
