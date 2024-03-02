using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Database.UnitsOfWork.Car;
using StandDigital.Core.Mappers;

namespace StandDigital.Core.RequestHandlers.UpdateCarDeal;

internal sealed class UpdateCarDealRequestHandler(
    ICarUoW carUoW,
    ILogger<UpdateCarDealRequestHandler> logger) : IRequestHandler<UpdateCarDealRequest, ErrorOr<Updated>>
{
    private readonly ICarUoW _carUoW = carUoW;
    private readonly ILogger<UpdateCarDealRequestHandler> _logger = logger;

    public async Task<ErrorOr<Updated>> Handle(UpdateCarDealRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        _logger.LogInformation("Start deal update process");

        var carEntity = request.Car.ToCardEntity();
        carEntity.CarroId = request.CarroId;
        var result = await _carUoW.UpdateCarAsync(carEntity, request.Photos, request.PrimaryPhoto, request.Extras, 
            request.CarMetadataEntity, cancellationToken);
        _logger.LogInformation("Finished deal update process with result {result}", result);
        return result;
    }
}
