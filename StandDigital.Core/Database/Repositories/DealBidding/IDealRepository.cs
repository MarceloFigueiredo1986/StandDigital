using ErrorOr;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;

namespace StandDigital.Core.Database.Repositories.DealBidding;

public interface IDealRepository
{
    Task<CarSellingPurposes?> GetDealLastBidingAsync(int carroId);
    Task<ErrorOr<Updated>> NewBidAsync(CarSellHistoryEntity newBid, CancellationToken cancellationToken);
}
