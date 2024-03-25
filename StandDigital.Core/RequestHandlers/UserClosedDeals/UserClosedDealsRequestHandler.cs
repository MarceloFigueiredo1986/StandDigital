using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.UserClosedDeals;

internal class UserClosedDealsRequestHandler(ICarRepository carRepository) : IRequestHandler<UserClosedDealsRequest, IEnumerable<ClosedDeals>>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<IEnumerable<ClosedDeals>> Handle(UserClosedDealsRequest request, CancellationToken cancellationToken)
        => await _carRepository.GetUserClosedDealsAsync(request.userId);
}
