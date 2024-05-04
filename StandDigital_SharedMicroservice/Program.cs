using StandDigital_Shared.Bootstrapping;
using StandDigital_Shared.Migrations;
using StandDigital_Shared.Repositories.CarBrandModels;
using StandDigital_Shared.Repositories.CarBrands;
using StandDigital_Shared.Repositories.CarExtras;
using StandDigital_SharedMicroservice.EndpointsServices;
using StandDigital_SharedMicroservice.HostedServices;
using StandDigital_SharedMicroservice.Migrations;

namespace StandDigital_SharedMicroservice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICarBrandModelRepository, CarBrandModelRepository>();
        builder.Services.AddScoped<ICarBrandRepository, CarBrandRepository>();
        builder.Services.AddScoped<ICarExtrasRepository, CarExtrasRepository>();
        builder.Services.InstallSharedProject();
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<IBrandModelService, BrandModelService>();
        builder.Services.AddScoped<IExtrasService, ExtrasService>();
        builder.Services.AddProblemDetails();
        builder.Services.AddScoped<IMigration,MigrationsService>();
        builder.Services.AddHostedService<DatabaseUpdate>();

        var app = builder.Build();
        app.EndpointsInstaller();
        app.Run();
    }
}
