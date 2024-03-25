using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.OpenDeals;

internal sealed class GetOpenDealsRequestHandler(ICarRepository carRepository) : IRequestHandler<GetOpenDealsRequest, IEnumerable<CarCardModel>>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<IEnumerable<CarCardModel>> Handle(GetOpenDealsRequest request, CancellationToken cancellationToken)
        => await _carRepository.GetOpenDealsByBusinessTypeAsync(request.tipoNegocio, request.userId);
}