using StandDigital.Entities;
using StandDigital.Models;
using StandDigital_Shared.Repositories;

namespace StandDigital.Repositories.Car;

public interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel);
}