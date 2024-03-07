using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.UserOpenDeals;

internal sealed class UserOpenDealsRequestHandler(ICarRepository carRepository) : IRequestHandler<UserOpenDealsRequest, IEnumerable<CarCompleteModel>>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<IEnumerable<CarCompleteModel>> Handle(UserOpenDealsRequest request, CancellationToken cancellationToken)
        => await _carRepository.GetUserDealsAsync(request.userId);
}
