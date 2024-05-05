using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.Database.UnitsOfWork.Car;

namespace StandDigital.Core.RequestHandlers.NewCarDeal;

internal sealed class NewCarDealRequestHandler(
    ICarUoW carUoW,
    ILogger<NewCarDealRequestHandler> logger) : IRequestHandler<NewCarDealRequest, ErrorOr<Created>>
{
    private readonly ICarUoW _carUoW = carUoW;
    private readonly ILogger<NewCarDealRequestHandler> _logger = logger;
    private const string missingPrimaryphoto = "Escolha a foto principal";

    public async Task<ErrorOr<Created>> Handle(NewCarDealRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        _logger.LogInformation("Start new deal creation process");

        if(!request.CarAdvertisement.Photos.Any(a => a.isPrimary))
        {
            _logger.LogWarning(missingPrimaryphoto);
            return Error.Validation(description: missingPrimaryphoto);
        }

        var carAdvertisment = new CarAdvertisementAggregate(
            request.CarAdvertisement.Car,
            request.CarAdvertisement.Photos,
            request.CarAdvertisement.Extras,
            request.CarAdvertisement.CarMetadataEntity);

        var result = await _carUoW.SaveNewCarAsync(carAdvertisment, cancellationToken);
        _logger.LogInformation("Finished new deal creation process with result {result}",result);
        return result;
    }
}
