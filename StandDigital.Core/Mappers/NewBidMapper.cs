using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.DealBiding;

namespace StandDigital.Core.Mappers;

public static class NewBidMapper
{
    public static CarSellHistoryEntity ToCarSellHistory(this AuctionRequest auction) =>
        new()
        {
            CarroId = auction.CarroId,
            CompradorId = auction.CompradorId,
            DataLicitacao = DateTime.UtcNow.ToString("o"),
            Valor = auction.Bid
        };
}
