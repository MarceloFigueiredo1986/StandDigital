using Blazored.Modal;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using StandDigital.Auth;
using StandDigital.Components.Components.ModalComponent;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.DeleteAdvertisement;
using StandDigital.Core.RequestHandlers.UserClosedDeals;
using StandDigital.Core.RequestHandlers.UserOpenDeals;

namespace StandDigital.Components.Pages.Admin.DealsArea;

public class PersonalDealsComponent : AbstractComponent<PersonalDealsComponent>
{
    protected ICollection<CarCompleteModel> OpenDeals = [];
    protected ICollection<ClosedDeals> ClosedDeals = [];


    [CascadingParameter]
    public int WindowWidth { get; set; }

    protected string? _marginStr;
    protected string? _float;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var session = await GetSessionKeyAsync<Authentication.AuthenticationData>(SessionSettings.SessionKeys.UserLoggedData);
            if (session is null) return;

            var resultOpenDeals = await _httpClient.SendAsync<ICollection<CarCompleteModel>>(
            HttpMethod.Get,
                service: StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.OpenDealsByUser,
                null,
                parameters: [session.IdUser.ToString()],
                cts.Token);

            var resultClosedDeals = await _httpClient.SendAsync<ICollection<ClosedDeals>>(
            HttpMethod.Get,
                service: StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.ClosedDealsByUser,
                null,
                parameters: [session.IdUser.ToString()],
                cts.Token);

            if (resultClosedDeals.IsError)
                _logger.LogError(resultClosedDeals.FirstError.Description);
            else
                ClosedDeals = resultClosedDeals.Value;

            if (resultOpenDeals.IsError)
                _logger.LogError(resultOpenDeals.FirstError.Description);
            else
                OpenDeals = resultOpenDeals.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null);
        }
    }

    protected void GotoCarDetails(int? carid = null) => navMan.NavigateTo($"managecar/{carid}");
    protected void CancelOpenDeal(int? carid = null)
    {
        var modalParames = new ModalParameters().
            Add("Message", "Deseja apagar o anúncio escolhido?").
            Add("YesHandler", EventCallback.Factory.Create<object>(this, DeleteAdvertisement));

        _modal.Show<ModalYesNo>(parameters: modalParames,new ModalOptions
        {
            Size = ModalSize.Small
        });
    }

    public async Task DeleteAdvertisement(object value)
    {
        try
        {
            var carId = (int)value;

            var result = await _httpClient.SendAsync<Deleted>(
                HttpMethod.Delete, 
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.DeleteDeal,
                null,
                [carId.ToString()], cts.Token);
            
            if(result.IsError)
            {
                _logger.LogError(result.FirstError.Description);
                _toaster.ShowError("Problema ao apagar anúncio");
            }

            _toaster.ShowSuccess("Anúncio cancelado.");
            navMan.NavigateTo(Path.Combine(navMan.BaseUri,"admin"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null);
            _toaster.ShowError("Problema ao apagar anúncio");
        }
    }

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
