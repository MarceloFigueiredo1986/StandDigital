using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.OpenDeals;

public record GetOpenDealsRequest(string tipoNegocio,int userId) : IRequest<IEnumerable<CarCardModel>>;
