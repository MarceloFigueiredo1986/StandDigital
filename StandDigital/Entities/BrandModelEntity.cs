namespace StandDigital.Entities;

public record BrandModelEntity(string Nome, int Cilindrada, int Cavalagem,int MarcaId)
{
    public BrandModelEntity() : this(default!,default,default,default)
    {}
}
