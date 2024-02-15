using StandDigital.Entities;
using StandDigital.Enums;
using StandDigital.Models;
using StandDigital_Shared.Repositories;

namespace StandDigital.Services.Repositories.Car;

public interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(Combustivel fuel);
    Task<CarWithExtrasAndImagesDto?> GetCompleteCarAsync(int carroId);
}