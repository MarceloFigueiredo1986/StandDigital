using StandDigital.Entities;
using StandDigital.Enums;
using StandDigital.Models;
using StandDigital_Shared.Repositories;

namespace StandDigital.Services.Repositories.Car;

public interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetOpenDealsAsync(EnumTiposNegocios tipoNegocio);
    Task<CarWithExtrasAndImagesDto?> GetCompleteCarAsync(int carroId);
    Task<int> SetPrimaryPhotoAsync(int carroId, int fotoId, CancellationToken cancellationToken);
}