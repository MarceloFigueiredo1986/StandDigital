using ErrorOr;
using MediatR;
using StandDigital.Core.Database.UnitsOfWork.Car;

namespace StandDigital.Core.RequestHandlers.DeleteAdvertisement;

internal sealed class DeleteAdvertisementRequestHandler(ICarUoW carUoW) : IRequestHandler<DeleteAdvertisementRequest, ErrorOr<Deleted>>
{
    public ICarUoW _carUoW { get; } = carUoW;

    public Task<ErrorOr<Deleted>> Handle(DeleteAdvertisementRequest request, CancellationToken cancellationToken)
        => _carUoW.DeleteCarAsync(request.CarId, cancellationToken);
}
