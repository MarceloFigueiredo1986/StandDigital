using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StandDigital.Auth;
using StandDigital.Settings;
using StandDigital_HttpClient.HttpService;

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
    protected IToastService _toaster { get; set; }

    [Inject]
    protected IMediator _mediatr { get; set; }

    [Inject]
    public AuthenticationStateProvider _authenticator { get; init; }

    [Inject]
    protected SessionSettings _session { get; init; }

    [Inject]
    public AppSettings _settings { get; init; }
}
