using ErrorOr;
using MediatR;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.UpdateCarDeal;

public sealed record UpdateCarDealRequest(int CarroId,
        CarAdvertisementCrudModel Car,
        Dictionary<string, byte[]> Photos,
        string? PrimaryPhoto,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity,
        CancellationToken CancellationToken) : IRequest<ErrorOr<Updated>>;