using Microsoft.AspNetCore.Components;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.HighlightedDeals;
using StandDigital.Core.RequestHandlers.OpenDeals;
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
                    Cars = await _mediatr.Send(new GetHighlightedDealsQuery());
                else
                    Cars = await _mediatr.Send(new GetOpenDealsRequest(TipoNegocio.GetDescription(), _userId));

                await InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, nameof(CarCarrosselService));
            _toaster.ShowWarning("Não foi possível carregar os negócios em destaque.");
        }
    }
}
