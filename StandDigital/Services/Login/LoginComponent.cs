using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StandDigital.Auth;
using StandDigital.Services.Repositories.User;
using StandDigital.Settings;

namespace StandDigital.Services;

public class LoginComponent : ComponentBase
{
    [Inject]
    protected ILogger<LoginComponent> _logger { get; init; }

    [Inject]
    protected SessionSettings _session { get; init; }

    [Inject]
    private IUserRepository _loginService { get; init; }

    [Inject]
    protected NavigationManager _navigate { get; init; }

    [Inject]
    public AuthenticationStateProvider _authenticator { get; init; }

    [Inject]
    public IToastService _toaster { get; init; }

    [Inject]
    public AppSettings _settings { get; init; }

    protected async Task<bool> ValidateLogin(string username,string password, 
        Authentication SignOutManager)
    {
        var result = await _loginService.ValidateLogin(username, password);


        if (result == null) return false;
           
        DateTime expirationDate = default;

#if DEBUG
        expirationDate = DateTime.UtcNow.AddDays(1);
#else
        expirationDate = DateTime.UtcNow.AddHours(12);
#endif

        var userData = new Authentication.AuthenticationData { 
            Name = result.Name is null ? result.UserId.ToString() : result.Name,
            ExpirationDate = expirationDate, 
            Role = result.Role,
            IdUser = result.UserId
        };
            
        await _session.AddKey(SessionSettings.SessionKeys.UserLoggedData, userData);
        SignOutManager.AuthenticateUser(userData);
        return true;
    }
}
