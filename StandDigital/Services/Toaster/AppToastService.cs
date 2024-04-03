using Blazored.Toast.Configuration;
using Blazored.Toast.Services;

namespace StandDigital.Services.Toaster;

public class AppToastService(IToastService toastService) : IAppToastService
{
    private readonly IToastService _toastService = toastService;

    private int _timer = 1;

    Action<ToastSettings> Action => a => {
        a.Position = ToastPosition.BottomLeft;
        a.Timeout = _timer;
        a.ShowProgressBar = true;
    };

    public void ShowError(string message, int timer = 5)
    {
        _timer = timer;
        _toastService.ShowError(message, Action);
    }

    public void ShowInfo(string message, int timer = 5)
    {
        _timer = timer;
        _toastService.ShowInfo(message, Action);
    }

    public void ShowSuccess(string message, int timer = 5)
    {
        _timer = timer;
        _toastService.ShowSuccess(message, Action);
    }

    public void ShowWarning(string message, int timer = 5)
    {
        _timer = timer;
        _toastService.ShowWarning(message, Action);
    }
}
