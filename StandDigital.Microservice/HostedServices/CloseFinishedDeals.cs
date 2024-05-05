using MediatR;
using StandDigital.Core.RequestHandlers.CloseFinishedDeals;

namespace StandDigital.Microservice.HostedServices;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                var mediator = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new CloseFinishedDealsRequest(), stoppingToken);

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
