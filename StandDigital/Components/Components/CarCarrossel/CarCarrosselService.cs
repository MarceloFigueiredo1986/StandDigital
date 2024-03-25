using Microsoft.AspNetCore.Components;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.OpenDeals;
using StandDigital.Enums;

namespace StandDigital.Components.Components.CarCarrossel;

public class CarCarrosselService : AbstractComponent<CarCarrosselService>
{
    [Parameter]
    public EnumTiposNegocios TipoNegocio { get; set; }
    protected IEnumerable<CarCardModel>? Cars;
    private int _userId;

    protected override async Task OnInitializedAsync()
    {
        (_, _userId) = await GetUserSessionDataAsync();
        Cars = await _mediatr.Send(new GetOpenDealsRequest(TipoNegocio.GetDescription(), _userId));
    }
}
