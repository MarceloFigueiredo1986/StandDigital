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
using StandDigital.Core.Encryption;
using StandDigital.Core.Mappers;

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

    public Task<ErrorOr<Deleted>> DeleteCarAsync(int carId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<CarAdvertisementAggregate>> GetCarAsync(int carId, CancellationToken cancellationToken)
    {
        try
        {
            var carTsk = _carRepository.GetCarDealDetailsAsync(carId);
            var photosTsk = _photosRepository.GetAllPhotosByCarAsync(carId, cancellationToken);
            var extrasTsk = _carExtrasRepository.GetAllExtrasByCarAsync(carId, cancellationToken);
            var carMetadataTsk = _carMetadataRepository.GetCarMetadaByCarAsync(carId, cancellationToken);
            var carWinningProposalTsk = _dealRepository.GetWinningBidAsync(carId);

            await Task.WhenAll(carTsk,photosTsk,extrasTsk,carMetadataTsk,carWinningProposalTsk);

            var winningProposal = carWinningProposalTsk.Result;

            if (winningProposal != default)
                winningProposal = winningProposal with
                {
                    Telefone = DataEncryption.Decrypt(winningProposal.Telefone)
                };

            return new CarAdvertisementAggregate(
                carTsk.Result??throw new Exception("Car not found"),
                photosTsk.Result.Select(a =>a.Data),
                extrasTsk.Result.ToHashSet(),
                carMetadataTsk.Result??throw new Exception("CarMetadata not found"),
                winningProposal);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }    
    }

    public async Task<ErrorOr<Created>> SaveNewCarAsync(
        CarAdvertisementAggregate carAdvertisement,
        CancellationToken cancellationToken)
    {
        try
        {
            await _database.OpenTransactionAsync();

            var carCreationResult = await _carRepository.CreateAsync(carAdvertisement.Car.ToCardEntity(), cancellationToken);

            if (carCreationResult.IsError)
                return Error.Failure(description: $"Car creation proccess:{carCreationResult.FirstError.Description}");

            carAdvertisement.CarMetadataEntity.CarroId = carCreationResult.Value;
            var carMetadataCreationResult = await _carMetadataRepository.CreateAsync(carAdvertisement.CarMetadataEntity, cancellationToken);

            if (carMetadataCreationResult.IsError)
                return Error.Failure(description: $"Car metadata creation proccess:{carMetadataCreationResult.FirstError.Description}");

            foreach (var pht in carAdvertisement.Photos)
            {
                var newPhoto = new CarPhotoEntity(carCreationResult.Value, pht.photoStream, pht.isPrimary);

                var photoCreationResult = await _photosRepository.CreateAsync(newPhoto, cancellationToken);

                if (photoCreationResult.IsError)
                    return photoCreationResult.FirstError;
            
                if(pht.isPrimary)
                {
                    var updated = await _carRepository.SetPrimaryPhotoAsync(carCreationResult.Value, photoCreationResult.Value, cancellationToken);
                    if (updated == 0)
                        return Error.Failure(description: $"Error defining photo as primary.");
                }
            }

            foreach (var extra in carAdvertisement.Extras)
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