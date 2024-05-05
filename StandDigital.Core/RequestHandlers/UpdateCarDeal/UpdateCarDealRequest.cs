using ErrorOr;
using MediatR;
using StandDigital.Core.Aggregates.CarAdvertisement;

namespace StandDigital.Core.RequestHandlers.UpdateCarDeal;

public sealed record UpdateCarDealRequest(int CarroId,
        CarAdvertisementAggregate Car,
        CancellationToken CancellationToken) : IRequest<ErrorOr<Updated>>;