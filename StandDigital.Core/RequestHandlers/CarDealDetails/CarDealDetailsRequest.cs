using ErrorOr;
using MediatR;
using StandDigital.Core.Aggregates.CarAdvertisement;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

public record CarDealDetailsRequest(int carId) : IRequest<ErrorOr<CarAdvertisementAggregate>>;
