using StandDigital_SharedMicroservice.Migrations;

namespace StandDigital_SharedMicroservice.HostedServices;

public class DatabaseUpdate : IHostedService
{
    private readonly ILogger<DatabaseUpdate> _logger;
    private readonly MigrationsService _migrationsService;

    public DatabaseUpdate(IServiceProvider serviceProvider)
    {
        var serviceScope = serviceProvider.CreateScope();
        _logger = serviceScope.ServiceProvider.GetService<ILogger<DatabaseUpdate>>();
        _migrationsService = serviceScope.ServiceProvider.GetService<MigrationsService>();   
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{hostedservicename} started", nameof(DatabaseUpdate));

        try
        {
           await _migrationsService.RunMigrations();
        
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
 