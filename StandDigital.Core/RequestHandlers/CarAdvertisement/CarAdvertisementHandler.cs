using ErrorOr;
using MediatR;

namespace StandDigital.Core.RequestHandlers.CarAdvertisement;

internal sealed class CarAdvertisementHandler : IRequestHandler<CarAdvertisementCrudModel, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(CarAdvertisementCrudModel request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
