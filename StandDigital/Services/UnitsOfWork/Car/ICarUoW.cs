using ErrorOr;
using StandDigital.Entities;

namespace StandDigital.Services.UnitsOfWork.Car;

public interface ICarUoW
{
    Task<ErrorOr<Created>> SaveNewCarAsync(
        CarEntity car,
        Dictionary<string, byte[]> photos,
        string? primaryPhoto,
        HashSet<string> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);

    Task<ErrorOr<Updated>> UpdateCarAsync(CarEntity car,
        Dictionary<string, byte[]> newPhotos,
        HashSet<string> deletedPhotos,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);
}
