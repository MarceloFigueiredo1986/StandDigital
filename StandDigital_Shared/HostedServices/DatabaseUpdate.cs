using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StandDigital_Shared.Migrations;

namespace StandDigital_SharedMicroservice.HostedServices;

public class DatabaseUpdate : IHostedService
{
    private readonly ILogger<DatabaseUpdate> _logger;
    private readonly IMigration _migrationsService;

    public DatabaseUpdate(IServiceProvider serviceProvider)
    {
        var serviceScope = serviceProvider.CreateScope();
        _logger = serviceScope.ServiceProvider.GetService<ILogger<DatabaseUpdate>>();
        _migrationsService = serviceScope.ServiceProvider.GetService<IMigration>();   
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{hostedservicename} started", nameof(DatabaseUpdate));

        try
        {
           await _migrationsService.RunMigrationsAsync();
        
        _logger.LogInformation("Migrations applied successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error or {hostedservicename}", nameof(DatabaseUpdate));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
       Task.FromResult(() => _logger.LogInformation("{hostedservicename} stopped", nameof(DatabaseUpdate)));
}
 