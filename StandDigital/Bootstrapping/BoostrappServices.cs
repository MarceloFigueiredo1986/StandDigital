using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using StandDigital.Auth;
using StandDigital.HostedServices;
using StandDigital.Services.Toaster;
using StandDigital.Settings;
using StandDigital_HttpClient;
using StandDigital_Shared.Bootstrapping;
using StandDigital_Shared.Persistence;

namespace StandDigital.Bootstrapping;

public static class BoostrappServices
{
    public static IServiceCollection InitializeSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(a => new AppSettings(configuration));
        services.AddScoped<IDatabase, Database>();
        services.InstallHttpClient();
        services.InstallSharedProject();
        services.AddHostedService<CloseFinishedDeals>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        services.AddScoped(a => new SessionSettings(a.GetRequiredService<ILocalStorageService>()));
        services.AddScoped<AuthenticationStateProvider, Authentication>();
        services.AddScoped<IAppToastService, AppToastService>();

        return services;
    }
}
