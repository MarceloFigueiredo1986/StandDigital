using StandDigital.Models;
using ErrorOr;
using StandDigital_Shared.Persistence;
using StandDigital.Entities;
using StandDigital.Enums;
using StandDigital.Extensions;

namespace StandDigital.Services.Repositories.Car;

public class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetOpenDealsAsync(EnumTiposNegocios tipoNegocio)
         => await _dbcontext.GetAsync<CarCardModel>("""
            SELECT 
            c.*,
            cf.Data as MainImage,
            cm.DataFimAnuncio
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId 
            INNER JOIN TipoNegocios tn on c.TipoNegocioId = tn.TipoNegocioId
            INNER JOIN CarrosFotos cf on c.FotoId = cf.FotoId
            WHERE strftime('%Y-%m-%d %H:%M:%S', DataFimAnuncio) > datetime('now')
            and Name = @tiponegocio;
            """, new Dictionary<string, object>() { 
             {
                 "@tiponegocio", tipoNegocio.GetDescription() 
             } });
    public async Task<IEnumerable<CarEntity>> GetAllAsync()
     => await _dbcontext.GetAsync<CarEntity>("""
            SELECT c.*,cf.Data as Photo
            FROM Carros c
            LEFT JOIN CarrosFotos cf ON c.CarroId = cf.CarroId AND c.FotoId = cf.FotoId
            """);

    public async Task<ErrorOr<int>> CreateAsync(CarEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarId = await _dbcontext.InsertAsyncAndGetIdAsync<CarEntity,int>("""
            INSERT INTO Carros
            (Marca,
            Modelo,
            Combustivel,
            FotoId,
            Kms,
            Cv,
            Bateria,
            TipoCarregamento,
            Cilindrada,
            Portas,
            MesRegisto,
            Lotacao,
            Caixa,
            Ano,
            Matricula,
            Comentarios,
            PrimeiroDono,
            Danos,
            ProblemasMotor,
            Nacional,
            PrecoBase,
            TipoNegocioId)
            VALUES 
            (
            @Marca,
            @Modelo,
            @Combustivel,
            @FotoId,
            @Kms,
            @Cv,
            @Bateria,
            @TipoCarregamento,
            @Cilindrada,
            @Portas,
            @MesRegisto,
            @Lotacao,
            @Caixa,
            @Ano,
            @Matricula,
            @Comentarios,
            @PrimeiroDono,
            @Danos,
            @ProblemasMotor,
            @Nacional,
            @PrecoBase,
            @TipoNegocioId);
            """, value, cancellationToken);

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
            Nacional,
            DataInicioAnuncio,
            DataFimAnuncio,
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
        var carSellHistory = await multipleResults.ReadSingleOrDefaultAsync<CarSellingPurposes>();

        var carPhotosBase64 = carPhotosBA?.Select(a => a.ImageStringBase64()!).ToArray();

        return new(Car: carDetails, Extras : carExtras.ToHashSet(), Photos : carPhotosBase64, CarSellHistory : carSellHistory);
    }

    public async Task<int> SetPrimaryPhotoAsync(int carroId, int fotoId, CancellationToken cancellationToken)
        => await _dbcontext.UpdateAsync("""
            UPDATE Carros
            SET FotoId = @fotoId
            WHERE CarroId = @carroId
            """,
            new Dictionary<string, object> {
                { "@carroId", carroId },
                { "@fotoId", fotoId}
            },
            cancellationToken);
}