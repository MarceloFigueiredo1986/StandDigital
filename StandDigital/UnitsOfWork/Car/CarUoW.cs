using StandDigital.Entities;
using StandDigital.Repositories.Car;
using StandDigital.Repositories.CarMetadata;
using StandDigital.Repositories.CarPhotos;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Persistence;
using StandDigital_Shared.Repositories.CarExtras;

namespace StandDigital.UnitsOfWork.Car;

public class CarUoW(
    ICarRepository carRepository,
    ICarMetadataRepository carMetadataRepository,
    IPhotosRepository photosRepository,
    ICarExtrasRepository carExtrasRepository,
    IDatabase database) : ICarUoW
{
    private readonly ICarRepository _carRepository = carRepository;
    private readonly IPhotosRepository _photosRepository = photosRepository;
    private readonly ICarExtrasRepository _carExtrasRepository = carExtrasRepository;
    private readonly IDatabase _database = database;

    private readonly ICarMetadataRepository _carMetadataRepository = carMetadataRepository;

    public async Task SaveNewCarAsync(
        CarEntity car,
        Dictionary<string, byte[]> photos,
        HashSet<int> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken)
    {
        try
        {
            _database.OpenTransaction();

            var carCreationResult = await _carRepository.CreateAsync(car, cancellationToken);

            if (carCreationResult.IsError)
                throw new Exception($"Car creation proccess:{carCreationResult.FirstError.Description}");

            carMetadataEntity.CarroId = carCreationResult.Value;
            var carMetadataCreationResult = await _carMetadataRepository.CreateAsync(carMetadataEntity, cancellationToken);

            if (carMetadataCreationResult.IsError)
                throw new Exception($"Car metadata creation proccess:{carMetadataCreationResult.FirstError.Description}");

            foreach(var pht in photos)
            {
                var newPhoto = new CarPhoto(carCreationResult.Value, pht.Key, pht.Value);

                var photosCreationResul = await _photosRepository.CreateAsync(newPhoto, cancellationToken);

                if (photosCreationResul.IsError)
                    throw new Exception($"Car photos creation proccess:{photosCreationResul.FirstError.Description}");
            }

            foreach(var extra in extras)
            {
                var newExtra = new CarExtra(extra,carCreationResult.Value);

                var photosCreationResul = await _carExtrasRepository.CreateAsync(newExtra, cancellationToken);

                if (photosCreationResul.IsError)
                    throw new Exception($"Car extra creation proccess:{photosCreationResul.FirstError.Description}");
            }

            _database.CommitTransaction();
        }
        catch
        {
            _database.RollbackTransaction();
            throw;
        }
    }

    public Task UpdateCarAsync(CarEntity car, Dictionary<string, byte[]> newPhotos, HashSet<string> deletedPhotos, CarMetadataEntity carMetadataEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
