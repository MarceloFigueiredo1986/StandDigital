namespace StandDigital.Core.Models;

public record ClosedDeals(int CarroId, string Marca, string Modelo, int Kms, int Cv, string Combustivel, byte[] MainImage,int TemProposta)
{
    public string? TipoCarregamento { get; init; }
    public int Bateria { get; init; }
    public int Cilindrada { get; init; }
    public string? Caixa { get; init; }

    public ClosedDeals() : this(default,default,default,default,default,default,default,default)
    {
        
    }
}
