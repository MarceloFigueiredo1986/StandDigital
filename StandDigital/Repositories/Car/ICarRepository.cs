using StandDigital.Entities;
using StandDigital.Models;

namespace StandDigital.Repositories.Car;

public interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel);
}