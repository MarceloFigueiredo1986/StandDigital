using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Database.UnitsOfWork.Car;
using StandDigital.Core.Mappers;

namespace StandDigital.Core.RequestHandlers.NewCarDeal;

internal sealed class NewCarDealRequestHandler(
    ICarUoW carUoW,
    ILogger<NewCarDealRequestHandler> logger) : IRequestHandler<NewCarDealRequest, ErrorOr<Created>>
{
    private readonly ICarUoW _carUoW = carUoW;
    private readonly ILogger<NewCarDealRequestHandler> _logger = logger;

    public async Task<ErrorOr<Created>> Handle(NewCarDealRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        _logger.LogInformation("Start new deal creation process");

        var carEntity = request.Car.ToCardEntity();
        var result = await _carUoW.SaveNewCarAsync(carEntity, request.Photos, request.PrimaryPhoto, request.Extras, request.CarMetadataEntity, cancellationToken);
        _logger.LogInformation("Finished new deal creation process with result {result}",result);
        return result;
    }
}
