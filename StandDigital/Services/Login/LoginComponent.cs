using StandDigital.Auth;
using StandDigital.Components;
using StandDigital.Core.RequestHandlers.ValidateLogin;

namespace StandDigital.Services;

public class LoginComponent : AbstractComponent<LoginComponent>
{

    protected async Task<bool> ValidateLogin(string username,string password, 
        Authentication SignOutManager)
    {
        var result = await _mediatr.Send(new ValidateLoginRequest(username, password));

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
            IdUser = result.UserId
        };
            
        await _session.AddKey(SessionSettings.SessionKeys.UserLoggedData, userData);
        SignOutManager.AuthenticateUser(userData);
        return true;
    }
}
