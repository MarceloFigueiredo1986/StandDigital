using ErrorOr;
using MediatR;

namespace StandDigital.Core.RequestHandlers.DeleteAdvertisement;

public record DeleteAdvertisementRequest(int CarId) : IRequest<ErrorOr<Deleted>>;
