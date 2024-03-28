using ErrorOr;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Entities;
using StandDigital.Core.Database.Repositories.CarExtras;
using StandDigital.Core.Database.Repositories.CarMetadata;
using StandDigital.Core.Database.Repositories.CarPhotos;
using StandDigital_Shared.Persistence;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Database.Repositories.DealBidding;
using StandDigital.Core.Models;
using StandDigital.Core.Aggregates.CarAdvertisement;

namespace StandDigital.Core.Database.UnitsOfWork.Car;

internal class CarUoW(
    ICarRepository carRepository,
    ICarMetadataRepository carMetadataRepository,
    IPhotosRepository photosRepository,
    ICarExtrasRepository carExtrasRepository,
    IDealRepository dealRepository,
    IDatabase database) : ICarUoW
{
    private readonly ICarRepository _carRepository = carRepository;
    private readonly IPhotosRepository _photosRepository = photosRepository;
    private readonly ICarExtrasRepository _carExtrasRepository = carExtrasRepository;
    private readonly IDealRepository _dealRepository = dealRepository;
    private readonly IDatabase _database = database;

    private readonly ICarMetadataRepository _carMetadataRepository = carMetadataRepository;

    public async Task<ErrorOr<CarAdvertisementAggregate>> GetCarAsync(int carId, CancellationToken cancellationToken)
    {
        try
        {
            var carTsk = _carRepository.GetCarDealDetailsAsync(carId, cancellationToken);
            var photosTsk = _photosRepository.GetAllPhotosByCarAsync(carId, cancellationToken);
            var extrasTsk = _carExtrasRepository.GetAllExtrasByCarAsync(carId, cancellationToken);
            var carMetadataTsk = _carMetadataRepository.GetCarMetadaByCarAsync(carId, cancellationToken);
            var carWinningProposalTsk = _dealRepository.GetWinningBidAsync(carId);

            await Task.WhenAll(carTsk,photosTsk,extrasTsk,carMetadataTsk,carWinningProposalTsk);

            return new CarAdvertisementAggregate(
                carTsk.Result??throw new Exception("Car not found"),
                photosTsk.Result.Select(a =>a.Data),
                extrasTsk.Result.ToHashSet(),
                carMetadataTsk.Result??throw new Exception("CarMetadata not found"),
                carWinningProposalTsk.Result);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }    
    }

    public async Task<ErrorOr<Created>> SaveNewCarAsync(
        CarEntity car,
        Dictionary<string, byte[]> photos,
        string primaryPhoto,
        HashSet<string> extras,
        CarMetadataEntity carMetadataEntity,
        CancellationToken cancellationToken)
    {
        try
        {
            await _database.OpenTransactionAsync();

            var carCreationResult = await _carRepository.CreateAsync(car, cancellationToken);

            if (carCreationResult.IsError)
                return Error.Failure(description: $"Car creation proccess:{carCreationResult.FirstError.Description}");

            carMetadataEntity.CarroId = carCreationResult.Value;
            var carMetadataCreationResult = await _carMetadataRepository.CreateAsync(carMetadataEntity, cancellationToken);

            if (carMetadataCreationResult.IsError)
                return Error.Failure(description: $"Car metadata creation proccess:{carMetadataCreationResult.FirstError.Description}");

            foreach (var pht in photos)
            {
                var isPrimaryPhoto = pht.Key == primaryPhoto;
                var newPhoto = new CarPhotoEntity(carCreationResult.Value, pht.Value, isPrimaryPhoto);

                var photoCreationResult = await _photosRepository.CreateAsync(newPhoto, cancellationToken);

                if (photoCreationResult.IsError)
                    return photoCreationResult.FirstError;
            
                if(isPrimaryPhoto)
                {
                    var updated = await _carRepository.SetPrimaryPhotoAsync(carCreationResult.Value, photoCreationResult.Value, cancellationToken);
                    if (updated == 0)
                        return Error.Failure(description: $"Set photo {pht.Key} as primary failed");
                }
            }

            foreach (var extra in extras)
            {
                var newExtra = new CarExtraEntity(0, carCreationResult.Value, extra);

                var photosCreationResul = await _carExtrasRepository.CreateAsync(newExtra, cancellationToken);

                if (photosCreationResul.IsError)
                    return photosCreationResul.FirstError;
            }

            _database.CommitTransaction();
        }
        catch
        {
            _database.RollbackTransaction();
            throw;
        }

        return Result.Created;
    }

    public Task<ErrorOr<Updated>> UpdateCarAsync(CarEntity car, Dictionary<string, byte[]> photos, string? primaryPhoto, HashSet<string> extras, CarMetadataEntity carMetadataEntity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}