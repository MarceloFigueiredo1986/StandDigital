using StandDigital.Models;
using ErrorOr;
using StandDigital_Shared.Persistence;
using StandDigital.Entities;
using StandDigital.Enums;

namespace StandDigital.Services.Repositories.Car;

public class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetTop5CarsByFuelAsync(Combustivel fuel)
         => await _dbcontext.GetAsync<CarCardModel>("""
            Select * from Carros c inner join CarrosMetadata cm on c.CarroId = cm.CarroId where Combustivel = @combustivel and cm.Ativo = 'true'
            """,
            new Dictionary<string, object>() { { "combustivel", fuel.GetDescription() } });

    public async Task<IEnumerable<CarEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarEntity>("""
            Select * from Carros
            """);

    public async Task<ErrorOr<int>> CreateAsync(CarEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarId = await _dbcontext.InsertAsyncAndGetIdAsync<CarEntity, int>("""
            INSERT INTO Carros
            VALUES 
            (
            @Marca,
            @Modelo,
            @Combustivel,
            @ImageMin,
            @Kms,
            @Cvs,
            @Bateria,
            @TipoCarregamento,
            @Cilindrada,
            @Portas,
            @Cor,
            @Preco,
            @MesRegisto,
            @Lotacao,
            @Caixa,
            @Condicao)
            """,
            value, cancellationToken);

            if (createdCarId == 0)
                return Error.Failure(description: "Car not created");

            return createdCarId;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex}");
        }
    }

    public async Task<CarWithExtrasAndImagesDto?> GetCompleteCarAsync(int carroId)
    {
        var queryCarDetails = """
            SELECT 
            c.CarroId,
            Bateria,
            TipoCarregamento,
            Cilindrada,
            Caixa,
            Combustivel,
            Marca,
            Modelo,
            Kms,
            MesRegisto,
            Ano,
            Danos,
            ProblemasMotor,
            ImageMin,
            Nacional,
            DataInicioAnuncio,
            PrecisaPneus,
            Comentarios,
            Matricula,
            Lotacao 
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId
            WHERE c.CarroId = @carroId;
            """;

        var queryCarExtras = """
            SELECT Extra FROM CarrosExtras WHERE CarroId = @carroId;
            """;

        var queryCarPhotos = """
            SELECT Data FROM CarrosFotos WHERE CarroId = @carroId;
            """;

        var querySellHistory = """
            select MAX(Valor) as BiggestBid,count(CarroId) as TotalBids,Name as Bidder 
            from HistoricoNegocios hn
            inner join Users u on hn.VendedorId = u.UserId
            where CarroId = @carroId
            group by CarroId,Name
            order by BiggestBid desc;
            """;


        var dynamicParms = new Dictionary<string, object>
        {
            { "carroId", carroId }
        };

        var multipleResults = await _dbcontext.GetAsync(
            queries: [queryCarDetails,queryCarExtras, queryCarPhotos, querySellHistory], 
            dynamicParameters: dynamicParms);

        var carDetails = multipleResults.ReadFirst<CarCompleteModel>();
        var carExtras = await multipleResults.ReadAsync<string>();
        var carPhotosBA = await multipleResults.ReadAsync<byte[]>();
        var carSellHistory = await multipleResults.ReadSingleAsync<CarSellingPurposes>();

        var carPhotosBase64 = carPhotosBA.Select(a => Convert.ToBase64String(a)).ToHashSet();

        return new(Car: carDetails, Extras : carExtras.ToHashSet(), Photos : carPhotosBase64, CarSellHistory : carSellHistory);
    }
}