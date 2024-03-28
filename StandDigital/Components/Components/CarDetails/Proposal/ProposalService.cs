using Microsoft.AspNetCore.Components;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.RequestHandlers.DealPayment;

namespace StandDigital.Components.Components.CarDetails.Proposal;

public class ProposalService : AbstractComponent<ProposalService>
{
    [Parameter]
    public CarSelledOffer? FinalPurposal { get; set; }

    [Parameter]
    public int CarroId { get; set; }

    CarPayingCode? _paymentDetails; 

    CancellationTokenSource cts = new();

    protected async Task GetTokenForPayment()
        => _paymentDetails = await _mediatr.Send(new DealPaymentQuery(CarroId), cts.Token);
}
