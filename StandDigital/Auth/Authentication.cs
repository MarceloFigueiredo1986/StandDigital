using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace StandDigital.Auth;

public class Authentication : AuthenticationStateProvider
{
    public class AuthenticationData
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Role { get; set; }
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymous = new ClaimsIdentity();
        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
    }

    public void AuthenticateUser(AuthenticationData _userSettings)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name,_userSettings.Name is null ? _userSettings.IdUser.ToString() : _userSettings.Name),
            new Claim(ClaimTypes.Expiration,_userSettings.ExpirationDate.ToString())
        }, "apiauth_type");

        if(_userSettings.Role != "none") 
            identity.AddClaim(new Claim(ClaimTypes.Role, _userSettings.Role));

        var Utilizador = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Utilizador)));
    }

    public void SignOut() => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
}
