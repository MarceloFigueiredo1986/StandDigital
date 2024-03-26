using StandDigital.Core.Entities;
using StandDigital_Shared.Repositories;

namespace StandDigital.Core.Database.Repositories.CarMetadata;

public interface ICarMetadataRepository : IBaseRepository<CarMetadataEntity>
{
    Task CloseFinishedDealsAsync(CancellationToken cancellationToken);
    Task<CarMetadataEntity?> GetCarMetadaByCarAsync(int carId, CancellationToken cancellationToken);
}
