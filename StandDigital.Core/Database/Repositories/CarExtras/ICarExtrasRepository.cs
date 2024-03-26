using StandDigital.Core.Entities;
using StandDigital_Shared.Repositories;

namespace StandDigital.Core.Database.Repositories.CarExtras;

internal interface ICarExtrasRepository : IBaseRepository<CarExtraEntity>
{
    Task<IEnumerable<string>> GetAllExtrasByCarAsync(int carId, CancellationToken cancellationToken);
}
