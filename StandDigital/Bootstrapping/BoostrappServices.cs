using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using StandDigital.Auth;
using StandDigital.Core.Database.Repositories.DealBidding;
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
        var sect = configuration.GetSection("Paths");
        services.AddSingleton(new AppSettings { LogoPath = sect["LogoPath"] });

        services.AddScoped<IDatabase, Database>();

        services.InstallHttpClient();
        services.InstallSharedProject();
        services.AddHostedService<DatabaseUpdate>();
        services.AddHostedService<CloseFinishedDeals>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        services.AddScoped(a => new SessionSettings(a.GetRequiredService<ILocalStorageService>()));
        services.AddScoped<AuthenticationStateProvider, Authentication>();
        services.AddScoped<IAppToastService, AppToastService>();
        services.AddAuthentication(configuration);


        return services;
    }
}
