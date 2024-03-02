using ErrorOr;
using StandDigital.Core.Entities;

namespace StandDigital.Core.Database.UnitsOfWork.Car;

internal interface ICarUoW
{
    Task<ErrorOr<Created>> SaveNewCarAsync(
        CarEntity car,
        Dictionary<string, byte[]> photos,
        string? primaryPhoto,
        HashSet<string> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);

    Task<ErrorOr<Updated>> UpdateCarAsync(CarEntity car,
        Dictionary<string, byte[]> photos,
        string? primaryPhoto,
        HashSet<string> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);
}
