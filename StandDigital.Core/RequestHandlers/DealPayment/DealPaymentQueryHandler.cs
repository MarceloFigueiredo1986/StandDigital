using MediatR;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.Database.Repositories.DealBidding;

namespace StandDigital.Core.RequestHandlers.DealPayment;

public sealed class DealPaymentQueryHandler(
    IDealRepository dealRepository) : IRequestHandler<DealPaymentQuery, CarPayingCode>
{
    private readonly IDealRepository _dealRepository = dealRepository;

    public async Task<CarPayingCode> Handle(DealPaymentQuery request, CancellationToken cancellationToken)
        => await _dealRepository.GetPaymentDetailsAsync(request.CarroId);
}
