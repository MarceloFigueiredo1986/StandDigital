using ErrorOr;
using MediatR;

namespace StandDigital.Core.RequestHandlers.DealBiding;

public record AuctionRequest(int CarroId, decimal Bid, int CompradorId) :
    IRequest<ErrorOr<Updated>>;
