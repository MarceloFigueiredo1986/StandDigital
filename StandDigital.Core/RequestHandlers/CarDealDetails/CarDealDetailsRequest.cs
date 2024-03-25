using ErrorOr;
using MediatR;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.CarDealDetails;

public record CarDealDetailsRequest(int carId) : IRequest<ErrorOr<(CarAdvertisementCrudModel Car,
        Dictionary<string, byte[]> Photos,
        string? PrimaryPhoto,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity,
        CancellationToken CancellationToken)>>;
