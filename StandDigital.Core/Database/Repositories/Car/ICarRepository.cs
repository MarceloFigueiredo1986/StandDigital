using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;
using StandDigital_Shared.Repositories;

namespace StandDigital.Core.Database.Repositories.Car;

internal interface ICarRepository : IBaseRepository<CarEntity>
{
    Task<IEnumerable<CarCardModel>> GetOpenDealsByBusinessTypeAsync(string tipoNegocio, int userId);
    Task<CarWithExtrasAndImagesDto?> GetCompleteCarAsync(int carroId);
    Task<int> SetPrimaryPhotoAsync(int carroId, int fotoId, CancellationToken cancellationToken);
    Task<IEnumerable<CarCompleteModel>> GetUserOpenDealsAsync(int userId);
    Task<IEnumerable<ClosedDeals>> GetUserClosedDealsAsync(int userId);
    Task<CarAdvertisementCrudModel?> GetCarDealDetailsAsync(int carId);
    Task<IEnumerable<CarCardModel>> GetHighlightedDealsAsync();
}