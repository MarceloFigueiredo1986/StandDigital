using StandDigital.Entities;
using StandDigital.Models;

namespace StandDigital.Repositories;

public interface ICarRepository
{
    Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(string fuel);
    Task<IEnumerable<CarEntity>> GetAllCardCarsAsync();
}