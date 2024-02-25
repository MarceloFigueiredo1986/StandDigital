using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StandDigital.Entities;
using StandDigital.Extensions;
using StandDigital.Models;
using StandDigital.Services.UnitsOfWork.Car;
using StandDigital_HttpClient.HttpService;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Services.Logging;

namespace StandDigital.Components.Components.CarDetails.ComponentsServices;

public class CarDetailsService : ComponentBase
{
    protected List<ImageUpload> TemporaryPhotos { get; set; } = new();
    protected CarEntity Car { get; set; } = new() { Marca = default, Combustivel = default, Modelo = default};
    protected CarMetadataEntity CarMetadata { get; set; } = new();

    /// <summary>
    /// Key- extra name
    /// </summary>
    public HashSet<string> Extras { get; set; } = [];

    #region DI
    [Inject]
    protected NavigationManager navMan { get; set; }

    [Inject]
    protected ICarUoW _carRepo { get; set; }

    [Inject]
    protected IWebHostEnvironment _env { get; set; }

    [Inject]
    protected IHttpService _httpClient { get; set; }

    [Inject]
    protected IConfiguration _configuration { get; set; }

    [Inject]
    protected ILogging _logger { get; set; }

    [Inject]
    protected IToastService _toaster { get; set; }

    #endregion

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

            isSaving = true;
            if (Carid is null)
                await CreateCar();
            else
                await EditCar();

            Car = null!;
            CarMetadata = null!;
            TemporaryPhotos = [];
            Extras.Clear();

            foreach (var photo in TemporaryPhotos)
                File.Delete(photo.AbsoluteNameWithPath);

            navMan.NavigateTo(Path.Combine(navMan.BaseUri, "admin"), true);

            _toaster.ShowSuccess("Anúncio criado!");
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            error = "Não foi possível guardar a alteração dos dados.";
        }
        finally
        {
            isSaving = false;
        }
    }
    protected async Task CreateCar()
        => await _carRepo.SaveNewCarAsync(
            Car, 
            await TemporaryPhotos.ResizePhotos(), 

            _primaryPhoto??throw new ArgumentNullException("Escolha a foto principal do anúncio"), 
            Extras, 
            CarMetadata, 
            cts.Token);

    protected Task EditCar()
    {
        throw new NotImplementedException();
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

        }
        catch (Exception ex)
        {
            _logger.Log(ex);
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
            _logger.Log(ex);
        }
    }
}
