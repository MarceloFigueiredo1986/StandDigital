using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.HighlightedDeals;

internal sealed class GetHighlightedDealsQueryHandler(ICarRepository carRepository) : 
    IRequestHandler<GetHighlightedDealsQuery, IEnumerable<CarCardModel>>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<IEnumerable<CarCardModel>> Handle(GetHighlightedDealsQuery request, CancellationToken cancellationToken)
        => await _carRepository.GetHighlightedDealsAsync();
}
