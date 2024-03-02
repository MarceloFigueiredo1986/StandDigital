using MediatR;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.UserOpenDeals;

public sealed record UserOpenDealsRequest(int userId) : IRequest<IEnumerable<CarEntity>>;
