using Blazored.LocalStorage;

namespace StandDigital.Auth;

public class SessionSettings
{
    ILocalStorageService localStorage;

    public SessionSettings(ILocalStorageService _localStorage) => localStorage = _localStorage;

    public enum SessionKeys
    {
        UserLoggedData
    }

    public async Task RemoveKey(SessionKeys sessionKey) => await localStorage.RemoveItemAsync(sessionKey.ToString());
    public async Task AddKey<T>(SessionKeys sessionKey,T obj) => await localStorage.SetItemAsync(sessionKey.ToString(),obj);
    public async Task<T> GetKey<T>(SessionKeys sessionKey) => await localStorage.GetItemAsync<T>(sessionKey.ToString());

    public async Task Logout()
    {
       await localStorage.ClearAsync();
    }
}
