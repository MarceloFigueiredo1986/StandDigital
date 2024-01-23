using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StandDigital.Entities;
using StandDigital.UnitsOfWork.Brand;
using StandDigital.UnitsOfWork.Car;
using StandDigital_Shared.Repositories.CarBrandModels;
using StandDigital_Shared.Repositories.CarBrands;

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
    public ICarBrandUoW _brandsData { get; set; }

    [Inject]
    public IWebHostEnvironment _env { get; set; }

    [Inject]
    public ICarBrandRepository _carBrandRepository { get; set; }

    [Inject]
    public ICarBrandModelRepository _carBrandModelsRepository { get; set; }

    #endregion

    protected string? error;
    protected bool isSaving;
    private CancellationTokenSource cts = new();

    //Default values for typeahead
    protected IEnumerable<string>? DefaultMarcas;
    protected (string Marca, string Combustivel, IEnumerable<string> Modelos)[]? DefaultModelos;
    protected IEnumerable<string>? DefaultExtras;

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
        var brandsLoad = async () =>
        {
            var result = await _carBrandRepository.GetAllAsync();
            if (!result.Any()) return;
            DefaultMarcas = result.Select(a => a.Name);
        };

        var modelsLoad = async () =>
        {
            var result = await _carBrandModelsRepository.GetAllAsync();
            if (!result.Any()) return;
            DefaultModelos = result.Select(a => a.Nome);
        };
    }
}
