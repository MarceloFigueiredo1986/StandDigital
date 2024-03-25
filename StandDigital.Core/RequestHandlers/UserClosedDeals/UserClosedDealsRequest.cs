using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.UserClosedDeals;

public sealed record UserClosedDealsRequest(int userId) : IRequest<IEnumerable<ClosedDeals>>;