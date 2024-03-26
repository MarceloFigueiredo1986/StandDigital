using StandDigital.Core.Entities;
using StandDigital_Shared.Repositories;

namespace StandDigital.Core.Database.Repositories.CarPhotos;

internal interface IPhotosRepository : IBaseRepository<CarPhotoEntity>
{
    Task<IEnumerable<CarPhotoEntity>> GetAllPhotosByCarAsync(int carId,CancellationToken cancellationToken);
}
