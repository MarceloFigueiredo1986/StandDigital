using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Database.UnitsOfWork.Car;
using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.NewCarDeal;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

internal sealed class CarDealDetailsRequestHandler(
    ICarUoW carUoW,
    ILogger<NewCarDealRequestHandler> logger) : IRequestHandler<CarDealDetailsRequest, 
        ErrorOr<(CarAdvertisementCrudModel Car,
        IEnumerable<byte[]> Photos,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity)>>
{
    private readonly ICarUoW _carUoW = carUoW;
    private readonly ILogger<NewCarDealRequestHandler> _logger = logger;

    public async Task<ErrorOr<(CarAdvertisementCrudModel? Car, 
        IEnumerable<byte[]> Photos, 
        HashSet<string> Extras, 
        CarMetadataEntity CarMetadataEntity)>> Handle(CarDealDetailsRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        return await _carUoW.GetCarAsync(request.carId, cancellationToken);
    }
}