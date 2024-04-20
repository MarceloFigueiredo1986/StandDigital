using ErrorOr;
using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.HighlightedDeals;

public record GetHighlightedDealsQuery() : IRequest<IEnumerable<CarCardModel>>;
