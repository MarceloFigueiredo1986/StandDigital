using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using StandDigital.Auth;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.CompleteCar;
using StandDigital.Core.RequestHandlers.DealBiding;
using System.Text.Json;

namespace StandDigital.Components.Pages.MarketPages.DetalhesNegocio;

public class DetalhesNegocioService : AbstractComponent<DetalhesNegocioService>
{
    [Parameter]
    public string TipoNegocio { get; set; }

    [Parameter]
    public int? CarroId { get; set; }

    protected CarWithExtrasAndImagesDto? _detalhesCarro;

    [CascadingParameter]
    public int WindowWidth { get; set; }

    protected bool isSaving;

    protected decimal _newBid;
    
    protected string? _WidthStr;
    
    protected string _sessionUser = string.Empty;

    protected int _sessionUserId;

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            if (CarroId != null)
            {
                _detalhesCarro = await _mediatr.Send(new GetCompleteCarRequest(CarroId.Value));

                if (_detalhesCarro is not null)
                    if (_detalhesCarro.CarSellHistory is not null)
                        _newBid = _detalhesCarro.CarSellHistory.BiggestBid + 1;
                    else
                        _newBid = _detalhesCarro.Car.PrecoBase + 1;

                var session_already_started = await _session.GetKey<Authentication.AuthenticationData>(SessionSettings.SessionKeys.UserLoggedData);
                _sessionUser = session_already_started.Name;
                _sessionUserId = session_already_started.IdUser;
            }
        }
        catch (SqliteException sqlex)
        {
            _logger.LogError("Exception:{exception}", sqlex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetCompleteCarAsync");
        }
    }

    CancellationTokenSource cts = new CancellationTokenSource();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (WindowWidth == 0 || _WidthStr is not null) return;

        _WidthStr = WindowWidth switch
        {
            0 => $"{400}px",
            >= 560 => $"{530}px",
            (> 350 and < 560) => $"{WindowWidth - 30}px",
            < 350 => $"{(WindowWidth - 10)}px",
            _ => $"{400}px",
        };

        await InvokeAsync(StateHasChanged);
    }
    protected async Task Bid()
    {
        isSaving = true;

        try
        {
            _ = _logger.BeginScope(_newBid);

            var result = await _mediatr.Send(new AuctionRequest(
                CarroId!.Value,
                _newBid,
                _sessionUserId
                ), cts.Token);

            if (result.IsError)
                if (result.FirstError.Type == ErrorOr.ErrorType.Conflict)
                {
                    _detalhesCarro = _detalhesCarro with
                    {
                        CarSellHistory = JsonSerializer.Deserialize<CarSellingPurposes>(result.FirstError.Metadata!.First().Value.ToString()!)
                    };

                    _newBid = _detalhesCarro.CarSellHistory.BiggestBid + 1;
                    _toaster.ShowWarning(result.FirstError.Description);
                }
                else
                    errorLog(new Exception(result.FirstError.Description));
            else
            {
                _toaster.ShowSuccess("Licitado com sucesso");
            }
        }
        catch (Exception ex)
        {
            errorLog(ex);
            _logger.LogError(ex, nameof(Bid));
            _toaster.ShowError("Não foi possível guardar a alteração dos dados.");
        }
        finally
        {
            isSaving = false;
        }

        void errorLog(Exception ex)
        {
            _logger.LogError(ex, nameof(Bid));
            _toaster.ShowError("Não foi possível guardar a alteração dos dados.");
        }
    }

    protected bool SelfAdvertisement()
        => _detalhesCarro.CarSellHistory?.Seller == _sessionUser;

    protected bool ShowLicitationButton() =>
        _detalhesCarro.CarSellHistory?.TotalBids == 0 || !SelfAdvertisement();
}
