using ErrorOr;
using MediatR;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

public record CarDealDetailsRequest(int carId) : IRequest<ErrorOr<(CarAdvertisementCrudModel Car,
        IEnumerable<byte[]> Photos,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity)>>;
