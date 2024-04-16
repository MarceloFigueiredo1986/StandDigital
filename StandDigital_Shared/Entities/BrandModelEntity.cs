namespace StandDigital_Shared.Entities;

public record BrandModelEntity(string Nome, int Cilindrada, int Cavalagem,string Combustivel,int MarcaId, string Segmento)
{
    public BrandModelEntity() : this(default!,default,default,default!,default, string.Empty)
    {}
}
