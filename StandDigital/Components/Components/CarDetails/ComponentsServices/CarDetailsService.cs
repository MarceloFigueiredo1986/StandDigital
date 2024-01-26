using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StandDigital.Entities;
using StandDigital.Services.UnitsOfWork.Car;
using StandDigital_HttpClient.HttpService;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Models;
using StandDigital_Shared.Services.Logging;

namespace StandDigital.Components.Components.CarDetails.ComponentsServices;

public class CarDetailsService : ComponentBase
{
    protected Dictionary<string, string>? TemporaryPhotos { get; set; } = new();
    protected CarEntity Car { get; set; } = new();
    protected CarMetadataEntity CarMetadata { get; set; } = new();

    /// <summary>
    /// Key- extra name, Value- if already exists in BD
    /// </summary>
    public Dictionary<string, bool> Extras { get; set; } = new();

    #region DI
    [Inject]
    public NavigationManager navMan { get; set; }

    [Inject]
    public ICarUoW _carRepo { get; set; }

    [Inject]
    public IWebHostEnvironment _env { get; set; }

    [Inject]
    public IHttpService _httpClient { get; set; }

    [Inject]
    public IConfiguration _configuration { get; set; }

    [Inject]
    public ILogging _logger { get; set; }

    #endregion

    protected string? error;
    protected bool isSaving;
    private CancellationTokenSource cts = new();

    //Default values for typeahead
    protected IEnumerable<BrandEntity>? DefaultMarcas;
    protected IEnumerable<BrandModelEntity>? DefaultModelos = Enumerable.Empty<BrandModelEntity>();
    protected IEnumerable<ExtraEntity>? DefaultExtras = Enumerable.Empty<ExtraEntity>();

    [Parameter]
    public int? Carid { get; set; }

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
            TemporaryPhotos = null;
            Extras.Clear();

            navMan.NavigateTo(Path.Combine(navMan.BaseUri, "admin"), true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            error = "Não foi possível guardar a alteração dos dados.";
        }
        finally
        {
            isSaving = false;
        }
    }
    protected async Task CreateCar()
        => await _carRepo.SaveNewCarAsync(Car, await GetPhotos(), default!, CarMetadata, cts.Token);

    async Task<Dictionary<string, byte[]>> GetPhotos()
    {
        var resultDic = new Dictionary<string, byte[]>();
        foreach (var photo in TemporaryPhotos)
        {
            using var ms = new MemoryStream();
            var path = Path.Combine(_env.ContentRootPath, "temporaryfiles", photo.Key);
            await using FileStream fs = new(path, FileMode.Open);
            await fs.CopyToAsync(ms);
            ms.Position = 0;
            resultDic.Add(photo.Value, ms.ToArray());
            File.Delete(Path.Combine(_env.ContentRootPath, "temporaryfiles",
                    photo.Key));
        }

        return resultDic;
    }

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
