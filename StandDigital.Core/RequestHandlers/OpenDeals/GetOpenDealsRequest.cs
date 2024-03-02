using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.OpenDeals;

public record GetOpenDealsRequest(string tipoNegocio) : IRequest<IEnumerable<CarCardModel>>;
