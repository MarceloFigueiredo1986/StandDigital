using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.UserOpenDeals;

internal sealed class UserOpenDealsRequestHandler(ICarRepository carRepository) : IRequestHandler<UserOpenDealsRequest, IEnumerable<CarEntity>>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<IEnumerable<CarEntity>> Handle(UserOpenDealsRequest request, CancellationToken cancellationToken)
        => await _carRepository.GetUserDealsAsync(request.userId);
}
