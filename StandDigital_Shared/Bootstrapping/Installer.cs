using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using StandDigital_Shared.Persistence;
using StandDigital_Shared.Repositories.CarBrandModels;
using StandDigital_Shared.Repositories.CarBrands;
using StandDigital_Shared.Services.Logging;

namespace StandDigital_Shared.Bootstrapping;

public static class Installer
{
    public static IServiceCollection InstallSharedProject(this IServiceCollection services)
    {
        services.AddScoped<ICarBrandModelRepository, CarBrandModelRepository>();
        services.AddScoped<ICarBrandRepository, CarBrandRepository>();
        services.AddScoped<IDatabase, Database>();
        services.AddScoped<ILogging, Logging>();
        services.AddLogging(config => config.AddSerilog());

        return services;
    }

    public static Logger InstallSerilogSinksConfiguration() =>
        new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.File($"logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
}
