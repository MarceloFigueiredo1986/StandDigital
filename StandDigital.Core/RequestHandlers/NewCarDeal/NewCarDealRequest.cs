using ErrorOr;
using MediatR;
using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.CarAdvertisement;

namespace StandDigital.Core.RequestHandlers.NewCarDeal;

public record NewCarDealRequest(CarAdvertisementCrudModel Car,
        Dictionary<string, byte[]> Photos,
        string? PrimaryPhoto,
        HashSet<string> Extras,
        CarMetadataEntity CarMetadataEntity,
        CancellationToken CancellationToken) : IRequest<ErrorOr<Created>>;