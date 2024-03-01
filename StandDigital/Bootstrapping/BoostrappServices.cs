using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Hosting;
using StandDigital.Auth;
using StandDigital.HostedServices;
using StandDigital.Services.Repositories.Car;
using StandDigital.Services.Repositories.CarExtras;
using StandDigital.Services.Repositories.CarMetadata;
using StandDigital.Services.Repositories.CarPhotos;
using StandDigital.Services.Repositories.TiposNegocios;
using StandDigital.Services.Repositories.User;
using StandDigital.Services.UnitsOfWork.Car;
using StandDigital.Settings;
using StandDigital_HttpClient;
using StandDigital_Shared.Bootstrapping;
using StandDigital_Shared.Persistence;
using static Dapper.SqlMapper;

namespace StandDigital.Bootstrapping;

public static class BoostrappServices
{
    public static IServiceCollection InitializeSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var sect = configuration.GetSection("Paths");
        services.AddSingleton(new AppSettings { LogoPath = sect["LogoPath"] });

        services.AddScoped<IDatabase, Database>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICarMetadataRepository, CarMetadataRepository>();
        services.AddScoped<IPhotosRepository, PhotosRepository>();
        services.AddScoped<ICarExtrasRepository, CarExtrasRepository>();
        services.AddScoped<ITiposNegociosRepository, TiposNegociosRepository>();
        services.AddScoped<ICarUoW, CarUoW>();
        services.InstallHttpClient();
        services.InstallSharedProject();
        services.AddHostedService<DatabaseUpdate>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        services.AddScoped(a => new SessionSettings(a.GetRequiredService<ILocalStorageService>()));
        services.AddScoped<AuthenticationStateProvider, Authentication>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
