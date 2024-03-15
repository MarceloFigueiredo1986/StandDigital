using ErrorOr;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;
using StandDigital_Shared.Persistence;

namespace StandDigital.Core.Database.Repositories.DealBidding;

public sealed class DealRepository(IDatabase database) : IDealRepository
{
    private readonly IDatabase _dbcontext = database;

    public async Task<CarSellingPurposes?> GetDealLastBidingAsync(int carroId)
        => await _dbcontext.GetSingleAsync<CarSellingPurposes>("""
            select MAX(Valor) as BiggestBid,count(hn.CarroId) as TotalBids,Name as Bidder 
            from HistoricoNegocios hn
            inner join CarrosMetadata cm on hn.CarroId = cm.CarroId
            inner join Users u on hn.CompradorId = u.UserId
            where hn.CarroId = @carroId
            group by hn.CarroId,Name
            order by BiggestBid desc;
            """, new Dictionary<string, object> { { "@carroId", carroId } });

    public async Task<ErrorOr<Updated>> NewBidAsync(CarSellHistoryEntity newBid, CancellationToken cancellationToken)
    {
        try
        {
            var CarPhotoId = await _dbcontext.InsertAsyncAndGetIdAsync<CarSellHistoryEntity,int>(
                     """
                    INSERT INTO 
                    HistoricoNegocios (CarroId,Valor,CompradorId,DataLicitacao)
                    VALUES (@CarroId,@Valor,@CompradorId,@DataLicitacao);
                    """, newBid, cancellationToken);

            if (CarPhotoId == 0)
                return Error.Failure(description: $"{nameof(CarSellHistoryEntity)} not created");

            return Result.Updated;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex}");
        }
    }
}
