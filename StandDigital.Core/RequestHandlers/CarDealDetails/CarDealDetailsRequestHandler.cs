using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Database.UnitsOfWork.Car;
using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.NewCarDeal;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

internal sealed class CarDealDetailsRequestHandler(
    ICarUoW carUoW,
    ILogger<NewCarDealRequestHandler> logger) : IRequestHandler<CarDealDetailsRequest, 
        ErrorOr<CarAdvertisementAggregate>>
{
    private readonly ICarUoW _carUoW = carUoW;
    private readonly ILogger<NewCarDealRequestHandler> _logger = logger;

    public async Task<ErrorOr<CarAdvertisementAggregate>> Handle(CarDealDetailsRequest request, CancellationToken cancellationToken)
    {
        _ = _logger.BeginScope(request);
        return await _carUoW.GetCarAsync(request.carId, cancellationToken);
    }
}