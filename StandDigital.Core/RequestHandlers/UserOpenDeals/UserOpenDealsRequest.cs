using MediatR;
using StandDigital.Core.Models;

namespace StandDigital.Core.RequestHandlers.UserOpenDeals;

public sealed record UserOpenDealsRequest(int userId) : IRequest<IEnumerable<CarCompleteModel>>;
