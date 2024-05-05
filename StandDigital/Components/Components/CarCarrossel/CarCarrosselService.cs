using Microsoft.AspNetCore.Components;
using StandDigital.Core.Models;
using StandDigital.Enums;

namespace StandDigital.Components.Components.CarCarrossel;

public class CarCarrosselService : AbstractComponent<CarCarrosselService>
{
    [Parameter]
    public EnumCarCarroselTipos TipoNegocio { get; set; }
    protected IEnumerable<CarCardModel>? Cars;
    private int _userId;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if(firstRender)
            {
                (_, _userId) = await GetUserSessionDataAsync();

                if (TipoNegocio == EnumCarCarroselTipos.Destaques)
                    await GetHighlightedDeals();
                else
                    await GetAllDealsByUser();

                await InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception ex)
        {
            DealWithError(ex.Message);
        }
    }

    async Task GetHighlightedDeals()
    {
        var result = await _httpClient.SendAsync<IEnumerable<CarCardModel>>(
            HttpMethod.Get,
            StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.HighlightedDeals,
            null,
            null,
            cts.Token);

        if (result.IsError)
        {
            DealWithError(result.FirstError.Description);
            return;
        }

        Cars = result.Value ?? [];
    }

    async Task GetAllDealsByUser()
    {
        var result = await _httpClient.SendAsync<IEnumerable<CarCardModel>>(
            HttpMethod.Get,
            StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.OpenDealsByUser,
            null,
            [_userId.ToString()],
            cts.Token);

        if (result.IsError)
        {
            DealWithError(result.FirstError.Description);
            return;
        }

        Cars = result.Value;
    }

    void DealWithError(string msg)
    {
        _logger.LogWarning(msg, nameof(CarCarrosselService));
        _toaster.ShowWarning("Não foi possível carregar os negócios em destaque.");
    }
}
