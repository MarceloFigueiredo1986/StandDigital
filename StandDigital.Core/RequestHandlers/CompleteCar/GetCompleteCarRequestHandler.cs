using MediatR;
using StandDigital.Core.Database.Repositories.Car;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.CompleteCar;

internal sealed class GetCompleteCarRequestHandler(ICarRepository carRepository) : IRequestHandler<GetCompleteCarRequest, CarWithExtrasAndImagesDto?>
{
    private readonly ICarRepository _carRepository = carRepository;

    public async Task<CarWithExtrasAndImagesDto?> Handle(GetCompleteCarRequest request, CancellationToken cancellationToken)
        => await _carRepository.GetCompleteCarAsync(request.CarroId);
}
