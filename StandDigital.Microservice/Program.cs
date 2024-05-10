
using StandDigital.Core;
using StandDigital.Microservice.HostedServices;
using StandDigital.Microservice.Middleware;
using StandDigital.Microservice.Migrations;
using StandDigital_Shared.Bootstrapping;
using StandDigital_Shared.Migrations;
using StandDigital_SharedMicroservice.HostedServices;

namespace StandDigital.Microservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.AddMemoryCache();
            builder.Services.InstallSharedProject();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Installer).Assembly));
            builder.Services.AddProblemDetails();
            builder.Services.AddScoped<IMigration,MigrationsService>();
            builder.Services.AddHostedService<DatabaseUpdate>();
            builder.Services.InstallExceptionMiddleware();
            builder.Services.InstallCoreDependencies();
            builder.Services.AddHostedService<CloseFinishedDeals>();

            var app = builder.Build();
            app.EndpointsInstaller();
            app.Run();
        }
    }
}
