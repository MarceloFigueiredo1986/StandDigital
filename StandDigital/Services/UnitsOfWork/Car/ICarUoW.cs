using StandDigital.Entities;

namespace StandDigital.Services.UnitsOfWork.Car;

public interface ICarUoW
{
    Task SaveNewCarAsync(
        CarEntity car,
        Dictionary<string, byte[]> photos,
        string primaryPhoto,
        HashSet<int> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);

    Task UpdateCarAsync(CarEntity car,
        Dictionary<string, byte[]> newPhotos,
        HashSet<string> deletedPhotos,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken);
}
