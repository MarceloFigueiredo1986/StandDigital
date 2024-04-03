using StandDigital.Core.Database.Repositories.CarMetadata;
using StandDigital.Core.Database.Repositories.DealBidding;

namespace StandDigital.HostedServices;

public class CloseFinishedDeals(
    IServiceProvider serviceProvider,
    ILogger<CloseFinishedDeals> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<CloseFinishedDeals> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(10)); //delay for migrations to run

        try
        {
            var scope = _serviceProvider.CreateScope();
            var carRepository = scope.ServiceProvider.GetRequiredService<ICarMetadataRepository>();
            var dealRepository = scope.ServiceProvider.GetRequiredService<IDealRepository>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await carRepository.CloseFinishedDealsAsync(stoppingToken);
                var generatedTokensSuccessfully = await dealRepository.GeneratePaymentTokenForClosedDealsAsync(stoppingToken);
                if (generatedTokensSuccessfully.IsError)
                    _logger.LogError("Error generating payment tokens.Error {Error}", generatedTokensSuccessfully.FirstError.Description);

                _logger.LogInformation("Deals Finished closing Service runned.");
                _logger.LogInformation("Deals Finish Service going to sleep.");

                await Task.Delay(TimeSpan.FromSeconds(30));
                _logger.LogInformation("Deals Finish Service wake up.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,nameof(CloseFinishedDeals));
        }
    }
}
