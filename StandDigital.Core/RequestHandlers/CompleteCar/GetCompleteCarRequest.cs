using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.CompleteCar;

public record GetCompleteCarRequest(int CarroId) : IRequest<CarWithExtrasAndImagesDto?>;