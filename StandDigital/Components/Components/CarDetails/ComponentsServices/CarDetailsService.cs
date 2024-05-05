using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StandDigital.Components.Components.CarDetails.Opcoes;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Mappers;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.CarDealDetails;
using StandDigital.Core.RequestHandlers.NewCarDeal;
using StandDigital.Core.RequestHandlers.TiposNegocios;
using StandDigital.Core.RequestHandlers.UpdateCarDeal;
using StandDigital.Enums;
using StandDigital.Extensions;
using StandDigital_Shared.Entities;

namespace StandDigital.Components.Components.CarDetails.ComponentsServices;

public class CarDetailsService : AbstractComponent<CarDetailsService>
{
    protected List<ImageUpload> TemporaryPhotos { get; set; } = new();
    protected CarAdvertisementCrudModel Car { get; set; } = new();
    protected CarMetadataEntity CarMetadata { get; set; } = new();
    protected CarSelledOffer? WinningProposal { get; set; }

    protected Tab _tabuladorEscolhido;

    protected IEnumerable<TipoNegocioEntity> TiposNegocios { get; set; } = [];

    /// <summary>
    /// Key- extra name
    /// </summary>
    public HashSet<string> Extras { get; set; } = [];

    protected string? error;
    protected bool isSaving;

    //Default values for typeahead
    protected IEnumerable<BrandEntity>? DefaultMarcas;
    protected IEnumerable<BrandModelEntity>? DefaultModelos = Enumerable.Empty<BrandModelEntity>();
    protected IEnumerable<ExtraEntity> DefaultExtras = Enumerable.Empty<ExtraEntity>();

    [Parameter]
    public int? Carid { get; set; }

    [Parameter]
    public string? Componente { get; set; }

    string? _primaryPhoto;

    protected static Dictionary<int, string> MesesAno = new() {
        { 0, "Escolher"},
        { 1, "Janeiro" },
        { 2, "Fevereiro" },
        { 3, "Março" },
        { 4, "Abril" },
        { 5, "Maio" },
        { 6, "Junho" },
        { 7, "Julho" },
        { 8, "Agosto" },
        { 9, "Setembro" },
        { 10, "Outubro" },
        { 11, "Novembro" },
        { 12, "Dezembro" },
    };

    protected enum Tab
    {
        Detalhes,
        Extras,
        EstadoViatura,
        Fotos,
        TipoNegocio
    }

    protected void GoBack() => navMan.NavigateTo("/admin");

    protected Type? MenuOpcoesTipoCombustivel;
    protected Dictionary<string, object> ParametrosComponenteOpcoesCombustivel => new(1) { { "Carro", Car } };

    protected async Task<IEnumerable<string>> SearchBrand(string searchText)
        => await Task.FromResult(
            DefaultMarcas!.
            Where(x =>
                x.Nome.ToLower().Contains(searchText.ToLower())).
                Select(a => a.Nome).
                Order().
                ToList());

    protected async Task<IEnumerable<string>> SearchModel(string searchText)
        => await Task.FromResult(DefaultModelos.
            Where(x => x.Nome.ToLower().
                    Contains(searchText.ToLower())).
            Select(a => a.Nome).
            Order().
            ToList());

    protected void SetCombustivel()
    {
        try
        {
            MenuOpcoesTipoCombustivel = Enum.Parse(typeof(Combustivel), Car.Combustivel) switch
            {
                Combustivel.Electrico => typeof(Eletrico),
                Combustivel.HibridoGasoleo or Combustivel.HibridoGasolina => typeof(Hibrido),
                _ => typeof(Combustao)
            };

            Car.Bateria = null;
            Car.Cilindrada = null;
            Car.Caixa = null;
            Car.TipoCarregamento = null;

            _logger.LogInformation(nameof(Car.Combustivel) + Car.Combustivel);

        }
        catch (Exception ex)
        {
            _logger.LogError($"error {ex.Message}");
        }
    }

    protected async Task CheckIfShouldClearModelsForLoading()
    {
        if (!string.IsNullOrEmpty(Car.Marca))
            await LoadModelsAsync();
    }
    protected async Task<string> DefaultWhenBrandNotFound(string value)
    {
        Car.Marca = value;
        return await Task.FromResult(value);
    }
    protected void ManageClickedExtra(string name)
    {
        if (Extras.Contains(name))
            Extras.Remove(name);
        else
            Extras.Add(name);
    }
    protected override async Task OnInitializedAsync()
        => await InitialLoadAsync();
    protected async Task ManageCommandToApply(EditContext editContext)
    {
        try
        {
            error = null;
            _primaryPhoto = TemporaryPhotos.FirstOrDefault(a => a.MainImage)?.ImageName;

            isSaving = true;

            if (Carid is null)
                await CreateNewAdvertisement();
            else
                await UpdateAdvertisement();

            Car = null!;
            CarMetadata = null!;
            TemporaryPhotos = [];
            Extras.Clear();

            foreach (var photo in TemporaryPhotos)
            {
                File.Delete(photo.AbsoluteNameWithPath);
                _logger.LogDebug("Foto {Photo} apagada", photo.AbsoluteNameWithPath);
            }

            navMan.NavigateTo(Path.Combine(navMan.BaseUri, "admin"), true);

            _toaster.ShowSuccess("Sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,nameof(ManageCommandToApply));
            error = "Não foi possível guardar a alteração dos dados.";
        }
        finally
        {
            isSaving = false;
        }
    }
    async Task CreateNewAdvertisement()
    {
        var carAdverAggr = new CarAdvertisementAggregate(
            Car, 
            await TemporaryPhotos.ResizePhotos(_primaryPhoto),
            Extras,
            CarMetadata);

        var result = await _httpClient.SendAsync<Created,CarAdvertisementAggregate>(
            HttpMethod.Post,
             StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.NewCarRequest,
             null,
             null,
             carAdverAggr,
             cts.Token);

        if (result.IsError)
            throw new Exception(result.FirstError.Description);

        if(result.Value != Result.Created)
            throw new Exception("Anúncio não foi criado.");
    }
    async Task UpdateAdvertisement()
    {
        var carAdvertisemnt = new CarAdvertisementAggregate(
            car: Car,
            photos: await TemporaryPhotos.ResizePhotos(_primaryPhoto),
            extras: Extras,
            carMetadataEntity: CarMetadata);

        var result = await _httpClient.SendAsync<ErrorOr<Updated>, CarAdvertisementAggregate>(
            method: HttpMethod.Put,
            service: StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.UpdateCarDeal,
            queryStringParams:null,
            parameters: null,
            bodyObj: carAdvertisemnt,
            cancellationToken: cts.Token
            );

        if (result.IsError)
            throw new Exception(result.FirstError.Description);

        if (result.Value != Result.Updated)
            throw new Exception("Anúncio não foi alterado.");
    }
    protected async Task InitialLoadAsync()
    {
        try
        {
            var result = await _httpClient.SendAsync<IEnumerable<BrandEntity>>(
            HttpMethod.Get,
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.AllBrands, null,null,cts.Token);

            if(result.IsError)
            {
                Console.WriteLine(result.FirstError.Description);
                return;
            }

            if (result.Value!.Any())
                DefaultMarcas = result.Value;

            var resultExtras = await _httpClient.SendAsync<IEnumerable<ExtraEntity>>(
            HttpMethod.Get,
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.AllExtras, null, null, cts.Token);

            if (resultExtras.IsError)
            {
                Console.WriteLine(resultExtras.FirstError.Description);
                return;
            }

            if (resultExtras.Value!.Any())
                DefaultExtras = resultExtras.Value;

            var resultTiposNegocios = await _httpClient.SendAsync<IEnumerable<TipoNegocioEntity>>(
            HttpMethod.Get,
                StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.AllDealTypes,
                null, null, cts.Token);

            TiposNegocios = resultTiposNegocios.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,nameof(InitialLoadAsync));
        }
    }
    protected async Task LoadModelsAsync()
    {
        _ = _logger.BeginScope(new {Car.Marca, Car.Combustivel });
        
        try
        {
            var marcaId = DefaultMarcas.First(a => a.Nome == Car.Marca).MarcaId;
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

            DefaultModelos = result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,nameof(LoadModelsAsync));
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            if (Carid != null)
            {
                var result = await _httpClient.SendAsync<CarAdvertisementAggregate>(
                    HttpMethod.Get,
                    StandDigital_HttpClient.HttpService.HttpServiceRequester.ServicesEndpoints.DealDetails,
                    null,
                    [Carid.Value.ToString()],
                    cts.Token);

                if(result.IsError)
                {
                    DealWithError(result.FirstError.Description);
                    return;
                }

                if(result.Value is null)
                {
                    _toaster.ShowWarning("Negócio não encontrado");
                    return;
                }

                var carDetails = result.Value;

                Car = carDetails.Car;
                Extras = carDetails.Extras;
                CarMetadata = carDetails.CarMetadataEntity;
                WinningProposal = carDetails.BuyingProposal;
            }
        }
        catch (Exception ex)
        {
            DealWithError(ex.Message);
        }
    
    
        void DealWithError(string msg)
        {
            _logger.LogError(msg, nameof(OnParametersSetAsync));
        }
    }
}