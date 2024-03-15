using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Database.Repositories.DealBidding;
using StandDigital.Core.Mappers;
using System.Text.Json;

namespace StandDigital.Core.RequestHandlers.DealBiding;

internal sealed class AuctionRequestHandler(
        IDealRepository dealRepository,
        ILogger<AuctionRequestHandler> logger) : IRequestHandler<AuctionRequest, ErrorOr<Updated>>
{
    private readonly IDealRepository _dealRepository = dealRepository;
    private readonly ILogger<AuctionRequestHandler> _logger = logger;

    public async Task<ErrorOr<Updated>> Handle(AuctionRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        _logger.LogInformation("New bid process handler start");

        var currentBid = await _dealRepository.GetDealLastBidingAsync(request.CarroId);

        if (currentBid is not null && currentBid.BiggestBid >= request.Bid)
            return Error.Conflict(
                description: "Proposta ultrapassada.",
                metadata: new Dictionary<string, object> { { "Bid", JsonSerializer.Serialize(currentBid) } });

        var carEntity = request.ToCarSellHistory();
        var result = await _dealRepository.NewBidAsync(carEntity, cancellationToken);
        _logger.LogInformation("Finished new bid process with result {result}", result);
        return result;
    }
}