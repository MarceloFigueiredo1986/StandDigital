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


    protected async Task GetTokenForPayment()
    {
        try
        {
            var result = await _httpClient.SendAsync<CarPayingCode>(
            HttpMethod.Get,
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.DealPayment,
                null,
                [CarroId.ToString()], 
                cts.Token);

            if(result.IsError)
            {
                _logger.LogWarning(result.FirstError.Description);
                _toaster.ShowWarning(result.FirstError.Description);
            }
            else
            {
                _toaster.ShowInfo($"Envie um pagamento MbWay para o número {result.Value.Telemovel} " +
                    $"com o código {result.Value.Codigo} para desbloquear a proposta vencedora", 60);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GetTokenForPayment));
            _toaster.ShowError(ex.Message);
        }
    }
}
