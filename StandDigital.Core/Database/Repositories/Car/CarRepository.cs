using ErrorOr;
using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Extensions;
using StandDigital.Core.Models;
using StandDigital_Shared.Persistence;

namespace StandDigital.Core.Database.Repositories.Car;

internal class CarRepository(IDatabase database) : ICarRepository
{
    private readonly IDatabase _dbcontext = database;
    public async Task<IEnumerable<CarCardModel>> GetOpenDealsByBusinessTypeAsync(string tipoNegocio, int userId)
         => await _dbcontext.GetAsync<CarCardModel>("""
            SELECT 
            c.*,
            cf.Data as MainImage,
            cm.DataInicioAnuncio
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId 
            INNER JOIN TipoNegocios tn on c.TipoNegocioId = tn.TipoNegocioId
            INNER JOIN CarrosFotos cf on c.FotoId = cf.FotoId
            WHERE DataFimAnuncio is null
            and Name = @tiponegocio and cm.VendedorId is not @userId;
            """, new Dictionary<string, object>() { 
             { "@tiponegocio", tipoNegocio },
             { "@userId", userId }
         });
    public async Task<IEnumerable<CarCompleteModel>> GetUserOpenDealsAsync(int userId)
     => await _dbcontext.GetAsync<CarCompleteModel>("""
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
            Cv,
            cf.Data as MainImage,
            cm.DataInicioAnuncio
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId
            LEFT JOIN CarrosFotos cf ON c.CarroId = cf.CarroId AND c.FotoId = cf.FotoId
            WHERE cm.VendedorId = @userId and cm.DataFimAnuncio is null
            """, new Dictionary<string, object>{ { "@userId", userId } });
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
            Lotacao,
            PrecoBase
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
            select MAX(Valor) as BiggestBid,count(hn.CarroId) as TotalBids,u.Name as Bidder,u2.Name as Seller 
            from CarrosMetadata cm
            left join HistoricoNegocios hn on hn.CarroId = cm.CarroId
            left join Users u on hn.CompradorId = u.UserId
            left join Users u2 on cm.VendedorId = u2.UserId
            where cm.CarroId = @carroId
            group by hn.CarroId,u.Name
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
    public Task<IEnumerable<CarEntity>> GetAllAsync() => throw new NotImplementedException();
    public async Task<IEnumerable<ClosedDeals>> GetUserClosedDealsAsync(int userId)
     => await _dbcontext.GetAsync<ClosedDeals>("""
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
            Cv,
            cf.Data as MainImage,
            cm.DataFimAnuncio,
            (select valor is not null from HistoricoNegocios where CarroId = c.CarroId) as TemProposta
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId
            LEFT JOIN CarrosFotos cf ON c.CarroId = cf.CarroId AND c.FotoId = cf.FotoId
            WHERE cm.VendedorId = @userId and cm.DataFimAnuncio not null
            """, new Dictionary<string, object> { { "@userId", userId } });
    public async Task<CarAdvertisementCrudModel?> GetCarDealDetailsAsync(int carId)
        => await _dbcontext.GetSingleAsync<CarAdvertisementCrudModel>("""
                    SELECT 
                    *
                    FROM Carros c 
                    WHERE c.CarroId = @carroId
                    """, new Dictionary<string, object> { { "@carroId", carId } });
    public async Task<IEnumerable<CarCardModel>> GetHighlightedDealsAsync()
        => await _dbcontext.GetAsync<CarCardModel>("""
            SELECT 
            c.*,
            cf.Data as MainImage,
            cm.DataInicioAnuncio
            FROM Carros c 
            INNER JOIN CarrosMetadata cm on c.CarroId = cm.CarroId 
            INNER JOIN TipoNegocios tn on cm.TipoNegocioId = tn.TipoNegocioId
            INNER JOIN CarrosFotos cf on c.FotoId = cf.FotoId
            INNER JOIN Destaques d on c.CarroId = d.CarroId
            WHERE datetime(DataFimDestaque) >= datetime('now')
            """);
}