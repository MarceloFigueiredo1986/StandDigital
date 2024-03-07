using ErrorOr;
using MediatR;
using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.CarAdvertisement;

namespace StandDigital.Core.RequestHandlers.UpdateCarDeal;

public sealed record UpdateCarDealRequest(int CarroId,
        CarAdvertisementCrudModel Car,
        Dictionary<string, byte[]> Photos,
        string? PrimaryPhoto,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity,
        CancellationToken CancellationToken) : IRequest<ErrorOr<Updated>>;