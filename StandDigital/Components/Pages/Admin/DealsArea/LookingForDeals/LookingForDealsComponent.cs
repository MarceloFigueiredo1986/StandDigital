using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using StandDigital.Core.RequestHandlers.LookingForDeals;
using StandDigital_Shared.Entities;
using System.Collections.Immutable;

namespace StandDigital.Components.Pages.Admin.DealsArea.LookingForDeals;

public class LookingForDealsComponent : AbstractComponent<LookingForDealsComponent>
{
    protected IEnumerable<BrandEntity>? _defaultMarcas;
    protected IEnumerable<BrandModelEntity>? _defaultModelos = Enumerable.Empty<BrandModelEntity>();
    protected ImmutableArray<string> Distritos => _configuration.GetSection("Filtros:Distritos").Get<string[]>()?.ToImmutableArray() ?? [];

    protected LookingForDealsQuery _lookingForDealsFormModel = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await _httpClient.SendAsync<IEnumerable<BrandEntity>>(
        HttpMethod.Get,
            StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.AllBrands, null, null, cts.Token);

        if (result.IsError)
        {
            Console.WriteLine(result.FirstError.Description);
            return;
        }

        if (result.Value!.Any())
            _defaultMarcas = result.Value;
    }

    protected async Task<IEnumerable<string>> SearchBrand(string searchText)
    => await Task.FromResult(
        _defaultMarcas!.
        Where(x =>
            x.Nome.ToLower().Contains(searchText.ToLower())).
            Select(a => a.Nome).
            Order().
            ToList());
    protected async Task<string> DefaultWhenBrandNotFound(string value)
    {
        _lookingForDealsFormModel.Marca = value;
        return await Task.FromResult(value);
    }
    protected Task ValidSubmitBrand(EditContext editContext)
        => Task.CompletedTask;

    
    protected async Task CheckIfShouldClearModelsForLoading()
    {
        if (!string.IsNullOrEmpty(_lookingForDealsFormModel.Marca))
            await LoadModelsAsync();
    }
    protected async Task LoadModelsAsync()
    {
        _ = _logger.BeginScope(new { _lookingForDealsFormModel.Marca });

        try
        {
            var marcaId = _defaultMarcas.First(a => a.Nome == _lookingForDealsFormModel.Marca).MarcaId;
            var result = await _httpClient.SendAsync<IEnumerable<BrandModelEntity>>(
            HttpMethod.Get,
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.BrandModels,
                queryStringParams: null,
                parameters: new()
                {
                    { marcaId.ToString() }
                },
                cts.Token);

            if (result.IsError)
            {
                Console.WriteLine(result.FirstError.Description);
                return;
            }

            _defaultModelos = result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(LoadModelsAsync));
        }
    }
    protected async Task<string> DefaultWhenModelNotFound(string value)
    {
        _lookingForDealsFormModel.Modelo = value;
        return await Task.FromResult(value);
    }
    protected async Task<IEnumerable<string>> SearchModel(string searchText)
    => await Task.FromResult(_defaultModelos.
        Where(x => x.Nome.ToLower().
                Contains(searchText.ToLower())).
        Select(a => a.Nome).
        Order().
        ToList());
}