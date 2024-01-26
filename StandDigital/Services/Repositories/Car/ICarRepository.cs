using StandDigital.Entities;
using StandDigital.Models;
using StandDigital_Shared.Repositories;

namespace StandDigital.Services.Repositories.Car;

public interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel);
}