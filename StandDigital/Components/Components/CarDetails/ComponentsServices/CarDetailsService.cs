using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;
using StandDigital.Core.RequestHandlers.NewCarDeal;
using StandDigital.Core.RequestHandlers.TiposNegocios;
using StandDigital.Core.RequestHandlers.UpdateCarDeal;
using StandDigital.Extensions;
using StandDigital_Shared.Entities;

namespace StandDigital.Components.Components.CarDetails.ComponentsServices;

public class CarDetailsService : AbstractComponent<CarDetailsService>
{
    protected List<ImageUpload> TemporaryPhotos { get; set; } = new();
    protected CarAdvertisementCrudModel Car { get; set; } = new();
    protected CarMetadataEntity CarMetadata { get; set; } = new();

    protected IEnumerable<TipoNegocioEntity> TiposNegocios { get; set; } = [];

    /// <summary>
    /// Key- extra name
    /// </summary>
    public HashSet<string> Extras { get; set; } = [];

    protected string? error;
    protected bool isSaving;
    private CancellationTokenSource cts = new();

    //Default values for typeahead
    protected IEnumerable<BrandEntity>? DefaultMarcas;
    protected IEnumerable<BrandModelEntity>? DefaultModelos = Enumerable.Empty<BrandModelEntity>();
    protected IEnumerable<ExtraEntity> DefaultExtras = Enumerable.Empty<ExtraEntity>();

    [Parameter]
    public int? Carid { get; set; }

    [Parameter]
    public string? Componente { get; set; }

    string? _primaryPhoto;

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
        var result = await _mediatr.Send(new NewCarDealRequest(
             Car,
             await TemporaryPhotos.ResizePhotos(),
             _primaryPhoto,
             Extras,
             CarMetadata,
             cts.Token));

        if (result.IsError)
            throw new Exception(result.FirstError.Description);

        if(result.Value != Result.Created)
            throw new Exception("Anúncio não foi criado.");
    }

    async Task UpdateAdvertisement()
    {
        var result = await _mediatr.Send(new UpdateCarDealRequest(
             Carid!.Value,
             Car,
             await TemporaryPhotos.ResizePhotos(),
             _primaryPhoto,
             Extras,
             CarMetadata,
             cts.Token));

        if (result.IsError)
            throw new Exception(result.FirstError.Description);

        if (result.Value != Result.Updated)
            throw new Exception("Anúncio não foi alterado.");
    }

    protected async Task InitialLoadAsync()
    {
        try
        {
            var brandMicroserviceUri = _configuration.GetValue<string>("AllBrandEndpoint") ?? throw new NullReferenceException("AllBrandEndpoint");
            var result = await _httpClient.SendAsync<IEnumerable<BrandEntity>>(brandMicroserviceUri, null,null,cts.Token);

            if(result.IsError)
            {
                Console.WriteLine(result.FirstError.Description);
                return;
            }

            if (result.Value!.Any())
                DefaultMarcas = result.Value;

            var extrasMicroserviceUri = _configuration.GetValue<string>("ExtrasModelsEndpoint") ?? throw new NullReferenceException("ExtrasModelsEndpoint");
            var resultExtras = await _httpClient.SendAsync<IEnumerable<ExtraEntity>>(extrasMicroserviceUri, null, null, cts.Token);

            if (resultExtras.IsError)
            {
                Console.WriteLine(resultExtras.FirstError.Description);
                return;
            }

            if (resultExtras.Value!.Any())
                DefaultExtras = resultExtras.Value;

            TiposNegocios = await _mediatr.Send(new GetTiposNegociosRequest());
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
            var brandMicroserviceUri = _configuration.GetValue<string>("BrandModelsEndpoint") ?? throw new NullReferenceException("BrandModelsEndpoint");
            var result = await _httpClient.SendAsync<IEnumerable<BrandModelEntity>>(
                endpointUrl: brandMicroserviceUri,
                queryStringParams: null,
                parameters: new()
                {
                    { marcaId.ToString() },
                    { Car.Combustivel }
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
}
