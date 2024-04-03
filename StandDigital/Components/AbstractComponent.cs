using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StandDigital.Auth;
using StandDigital.Services.Toaster;
using StandDigital.Settings;
using StandDigital_HttpClient.HttpService;
using static StandDigital.Auth.SessionSettings;

namespace StandDigital.Components;

public class AbstractComponent<T> : ComponentBase
{
    [Inject]
    protected NavigationManager navMan { get; set; }

    [Inject]
    protected IWebHostEnvironment _env { get; set; }

    [Inject]
    protected IHttpService _httpClient { get; set; }

    [Inject]
    protected IConfiguration _configuration { get; set; }

    [Inject]
    protected ILogger<T> _logger { get; set; }

    [Inject]
    protected IAppToastService _toaster { get; set; }

    [Inject]
    protected IMediator _mediatr { get; set; }

    [Inject]
    public AuthenticationStateProvider _authenticator { get; init; }

    [Inject]
    private SessionSettings _session { get; init; }

    [Inject]
    public AppSettings _settings { get; init; }

    [CascadingParameter]
    public IModalService _modal { get; set; } = default!;

    protected async Task<(string user, int userId)> GetUserSessionDataAsync()
    {
        var session_already_started = await _session.GetKey<Authentication.AuthenticationData>(SessionSettings.SessionKeys.UserLoggedData);
        return (session_already_started.Name, session_already_started.IdUser);
    }

    protected async Task AddSessionKeyAsync<T1>(SessionKeys sessionKey, T1 obj)
        => await _session.AddKey(sessionKey, obj);

    protected async Task<T1> GetSessionKeyAsync<T1>(SessionKeys sessionKey)
        => await _session.GetKey<T1>(sessionKey);
}
