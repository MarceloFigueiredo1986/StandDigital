using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using static StandDigital.Components.Pages.Login.Login;
using StandDigital.Core.RequestHandlers.User;

namespace StandDigital.Components.Pages.Login.CriarConta;

public class CriarContaComponent : AbstractComponent<CriarContaComponent>
{
    protected UserRequest _user = new();
    protected bool _isSaving;
    protected string? _error;

    [Parameter]
    public MenusLogin ChoosedMenu { get; set; }

    [Parameter]
    public EventCallback<MenusLogin> ChoosedMenuChanged { get; set; }

    protected async Task Save(EditContext editContext)
    {
        try
        {
            _ = _logger.BeginScope(_user);

            var result = await _mediatr.Send(_user);

            if(result.IsError)
            {
                _logger.LogError("Utilizador não criado.{Erro}", result.FirstError.Description);
                _toaster.ShowError(result.FirstError.Description);
                return;
            }

            if(!result.Value)
                _logger.LogError("Utilizador não criado.");
            else
                _logger.LogInformation("Utilizador criado com sucesso");

            ChoosedMenu = MenusLogin.Login;
            await ChoosedMenuChanged.InvokeAsync(ChoosedMenu);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registar utilizador");
            _error = ex.Message;
        }
        finally
        {
            _isSaving = false;
        }
    }
}
