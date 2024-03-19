using Microsoft.AspNetCore.Components;
using StandDigital.Auth;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.UserOpenDeals;

namespace StandDigital.Components.Pages.Admin.DealsArea;

public class PersonalOpenDealsComponent : AbstractComponent<PersonalOpenDealsComponent>
{
    protected ICollection<CarCompleteModel> OpenDeals = [];
    protected ICollection<CarCompleteModel> ClosedDeals = [];

    [CascadingParameter]
    public int WindowWidth { get; set; }

    protected string? _marginStr;
    protected string? _float;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var session = await _session.GetKey<Authentication.AuthenticationData>(SessionSettings.SessionKeys.UserLoggedData);
            OpenDeals = (await _mediatr.Send(new UserOpenDealsRequest(session.IdUser))).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null);
        }
    }

    protected void GotoCarDetails(int? carid = null) => navMan.NavigateTo($"managecar/{carid}");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (WindowWidth == 0 || _marginStr is not null) return;

        (_marginStr, _float) = WindowWidth switch
        {
            > 560 => ($"{15}px", "left"),
            <= 560 => ($"auto", "none")
        };

        await InvokeAsync(StateHasChanged);
    }

}
