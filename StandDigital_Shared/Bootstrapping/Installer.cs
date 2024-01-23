using Microsoft.Extensions.DependencyInjection;
using StandDigital_Shared.Persistence;
using StandDigital_Shared.Repositories.CarBrandModels;
using StandDigital_Shared.Repositories.CarBrands;
using StandDigital_Shared.Repositories.CarExtras;

namespace StandDigital_Shared.Bootstrapping;

public static class Installer
{
    public static IServiceCollection InstallSharedProject(this IServiceCollection services)
    {
        services.AddScoped<ICarBrandModelRepository, CarBrandModelRepository>();
        services.AddScoped<ICarBrandRepository, CarBrandRepository>();
        services.AddScoped<ICarExtrasRepository, CarExtrasRepository>();
        services.AddScoped<IDatabase, Database>();
        return services;
    }
}
