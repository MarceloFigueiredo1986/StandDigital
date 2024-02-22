using StandDigital.Entities;
using StandDigital.Services.Repositories.Car;
using StandDigital.Services.Repositories.CarExtras;
using StandDigital.Services.Repositories.CarMetadata;
using StandDigital.Services.Repositories.CarPhotos;
using StandDigital_Shared.Persistence;

namespace StandDigital.Services.UnitsOfWork.Car;

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
        string primaryPhoto,
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

            foreach (var pht in photos)
            {
                var isPrimaryPhoto = pht.Key == primaryPhoto;
                var newPhoto = new CarPhotoEntity(carCreationResult.Value, pht.Key, pht.Value, isPrimaryPhoto);

                var photoCreationResult = await _photosRepository.CreateAsync(newPhoto, cancellationToken);

                if (photoCreationResult.IsError)
                    throw new Exception($"Car photos creation proccess:{photoCreationResult.FirstError.Description}");
            
                if(isPrimaryPhoto)
                {
                    var updated = await _carRepository.SetPrimaryPhotoAsync(car.CarroId, photoCreationResult.Value, cancellationToken);
                    if (updated == 0)
                        throw new Exception($"Set primary photo failed");
                }
            }

            foreach (var extra in extras)
            {
                var newExtra = new CarExtraEntity(extra, carCreationResult.Value);

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
