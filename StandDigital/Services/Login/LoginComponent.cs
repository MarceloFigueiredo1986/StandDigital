using StandDigital.Auth;
using StandDigital.Components;
using StandDigital.Core.Entities;

namespace StandDigital.Services;

public class LoginComponent : AbstractComponent<LoginComponent>
{

    protected async Task<bool> ValidateLogin(string username,string password, 
        Authentication SignOutManager)
    {
        var result = await _httpClient.SendAsync<UserEntity>(StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.ValidateLogin,
            null, [username,password], cts.Token);

        if (result.Value == null) return false;
           
        DateTime expirationDate = default;

#if DEBUG
        expirationDate = DateTime.UtcNow.AddDays(1);
#else
        expirationDate = DateTime.UtcNow.AddHours(12);
#endif

        var userData = new Authentication.AuthenticationData { 
            Name = result.Value.Name is null ? result.Value.UserId.ToString() : result.Value.Name,
            ExpirationDate = expirationDate, 
            Role = "default",
            IdUser = result.Value.UserId
        };
            
        await AddSessionKeyAsync(SessionSettings.SessionKeys.UserLoggedData, userData);
        SignOutManager.AuthenticateUser(userData);
        return true;
    }
}
