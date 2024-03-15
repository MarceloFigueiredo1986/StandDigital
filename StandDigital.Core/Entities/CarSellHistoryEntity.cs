namespace StandDigital.Core.Entities;

public class CarSellHistoryEntity
{
    public int NegocioId { get; init; }
    public int CarroId { get; init; }
    public decimal Valor { get; init; }
    public string DataLicitacao { get; init; }
    public int CompradorId { get; init; }
}
