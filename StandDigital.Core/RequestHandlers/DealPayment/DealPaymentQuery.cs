
using ErrorOr;
using MediatR;
using StandDigital.Core.Aggregates.CarAdvertisement;

namespace StandDigital.Core.RequestHandlers.DealPayment;

public record DealPaymentQuery(int CarroId) : IRequest<ErrorOr<CarPayingCode>>;
