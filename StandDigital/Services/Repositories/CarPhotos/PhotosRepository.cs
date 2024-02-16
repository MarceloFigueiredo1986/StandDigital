using ErrorOr;
using StandDigital.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Services.Repositories.CarPhotos;

public class PhotosRepository(IDatabase database) : IPhotosRepository
{
    private readonly IDatabase _dbcontext = database;

    public async Task<ErrorOr<int>> CreateAsync(CarPhotoEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarPhotoId = await _dbcontext.InsertAsyncAndGetIdAsync<CarPhotoEntity, int>(
                     """
            INSERT INTO 
            CarrosFotos
            VALUES
            (
            @CarroId,
            @Name,
            @Data)
            """, value, cancellationToken);

            if (createdCarPhotoId == 0)
                throw new Exception($"{nameof(CarPhotoEntity)} not created");

            return createdCarPhotoId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<CarPhotoEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarPhotoEntity>("""
            Select * from CarrosFotos
            """);
}
