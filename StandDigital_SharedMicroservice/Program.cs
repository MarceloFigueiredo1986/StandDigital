using StandDigital_Shared.Bootstrapping;
using StandDigital_SharedMicroservice.EndpointsServices;
using System.Net;

namespace StandDigital_SharedMicroservice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.AddMemoryCache();
        builder.Services.InstallSharedProject();
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<IBrandModelService, BrandModelService>();
        builder.Services.AddScoped<IExtrasService, ExtrasService>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();
        app.EndpointsInstaller();
        app.Run();
    }
}
