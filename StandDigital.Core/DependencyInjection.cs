using Microsoft.Extensions.DependencyInjection;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Database.Repositories.CarExtras;
using StandDigital.Core.Database.Repositories.CarMetadata;
using StandDigital.Core.Database.Repositories.CarPhotos;
using StandDigital.Core.Database.Repositories.DealBidding;
using StandDigital.Core.Database.Repositories.TiposNegocios;
using StandDigital.Core.Database.Repositories.User;
using StandDigital.Core.Database.UnitsOfWork.Car;

namespace StandDigital.Core;

public static class DependencyInjection
{
    public static IServiceCollection InstallCoreDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICarMetadataRepository, CarMetadataRepository>();
        services.AddScoped<IPhotosRepository, PhotosRepository>();
        services.AddScoped<ICarExtrasRepository, CarExtrasRepository>();
        services.AddScoped<ITiposNegociosRepository, TiposNegociosRepository>();
        services.AddScoped<ICarUoW, CarUoW>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDealRepository, DealRepository>();

        return services;
    }
}
