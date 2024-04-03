using ErrorOr;
using Microsoft.Extensions.Configuration;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;
using StandDigital_Shared.Persistence;
using System.Text;

namespace StandDigital.Core.Database.Repositories.DealBidding;

public sealed class DealRepository(IDatabase database,IConfiguration configuration) : IDealRepository
{
    private readonly IDatabase _dbcontext = database;
    private readonly string? _paymentMobile = configuration["TelemovelPagamento"];

    public async Task<ErrorOr<Success>> GeneratePaymentTokenForClosedDealsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var closedDealsWithPurposalAndWithoutPaymentTokens = await _dbcontext.GetAsync<string>("""
                SELECT 
                DISTINCT cm.CarroId
                FROM CarrosMetadata cm
                LEFT JOIN Pagamentos p on cm.CarroId = p.CarroId 
                LEFT JOIN HistoricoNegocios h on cm.CarroId = h.CarroId 
                WHERE p.CarroId is null and h.CarroId is not null
                """);

            if (!closedDealsWithPurposalAndWithoutPaymentTokens.Any())
                return Result.Success;

            var queryInsertPayments = new StringBuilder("""
                INSERT INTO Pagamentos (CarroId,TokenPagamento,DataCriacao) 
                VALUES 
                """);

            var resultsArray = closedDealsWithPurposalAndWithoutPaymentTokens.ToArray();

            var dic = new Dictionary<string, object>();

            var creationDate = DateTimeOffset.UtcNow.ToString("yyyyMMdd");

            for (var incr = 0;incr != resultsArray.Length;incr++)
            {
                var carId = resultsArray[incr];
                queryInsertPayments.Append($" (@parm1{incr},@parm2{incr},@parm3{incr}) {(resultsArray.Length -1 == incr ? string.Empty : ",")}");
                dic.Add($"@parm1{incr}", carId);
                dic.Add($"@parm2{incr}", $"{carId}_{creationDate}");
                dic.Add($"@parm3{incr}", creationDate);
            }

            var insertedRows = await _dbcontext.InsertAsync(queryInsertPayments.ToString(), parameters: dic, cancellationToken);

            if (insertedRows == closedDealsWithPurposalAndWithoutPaymentTokens.Count())
                return Result.Success;
            else
                return Error.Failure(description: "Failed the insert of payment tokens");
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex}");
        }
    }

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

    public async Task<ErrorOr<CarPayingCode>> GetPaymentDetailsAsync(int carroId)
    {
        try
        {
            var PaymentToken = await _dbcontext.GetSingleAsync<string>(
                     """
                    Select TokenPagamento FROM Pagamentos WHERE CarroId = @carroId
                    """, new Dictionary<string, object> { { "@carroId", carroId } });

            ArgumentException.ThrowIfNullOrEmpty(PaymentToken);

            return new CarPayingCode(_paymentMobile, PaymentToken);
        }
        catch (Exception ex)
        {
            return Error.Failure(description: $"{ex}");
        }
    }

    public async Task<CarSelledOffer?> GetWinningBidAsync(int carroId)
        => await _dbcontext.GetSingleAsync<CarSelledOffer>("""
            select MAX(Valor) as Offer,Paid,Name as Buyer,Mobile as Telefone  
            from HistoricoNegocios hn
            inner join CarrosMetadata cm on hn.CarroId = cm.CarroId
            inner join Users u on hn.CompradorId = u.UserId
            where hn.CarroId = @carroId and cm.DataFimAnuncio is not null
            group by hn.CarroId,Name
            order by Offer desc;
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
