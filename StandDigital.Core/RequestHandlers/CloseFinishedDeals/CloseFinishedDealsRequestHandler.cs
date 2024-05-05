using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Database.Repositories.CarMetadata;
using StandDigital.Core.Database.Repositories.DealBidding;

namespace StandDigital.Core.RequestHandlers.CloseFinishedDeals;

internal sealed class CloseFinishedDealsRequestHandler(
    ILogger<CloseFinishedDealsRequestHandler> logger,
    ICarMetadataRepository carMetadataRepository,
    IDealRepository dealRepository) : IRequestHandler<CloseFinishedDealsRequest>
{
    public async Task Handle(CloseFinishedDealsRequest request, CancellationToken cancellationToken)
    {
        try
		{
            _ = logger.BeginScope("Start of closing deals and generation of payment tokens process");

            await carMetadataRepository.CloseFinishedDealsAsync(cancellationToken);
            logger.LogInformation("Deals closed successfully");

            var generatedTokensSuccessfully = await dealRepository.GeneratePaymentTokenForClosedDealsAsync(cancellationToken);
            if (generatedTokensSuccessfully.IsError)
                logger.LogError("Error generating payment tokens.Error {Error}", generatedTokensSuccessfully.FirstError.Description);
            else
                logger.LogInformation("Payments tokens generated successfully");
        }
        catch (Exception ex)
		{
            logger.LogError(ex, nameof(CloseFinishedDeals));
		}
    }
}
