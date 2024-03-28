using ErrorOr;
using MediatR;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

public record CarDealDetailsRequest(int carId) : IRequest<ErrorOr<CarAdvertisementAggregate>>;
