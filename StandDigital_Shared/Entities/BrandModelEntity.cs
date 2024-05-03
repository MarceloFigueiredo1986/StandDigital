namespace StandDigital_Shared.Entities;

public record BrandModelEntity(string Nome,int MarcaId)
{
    public BrandModelEntity() : this(default!,default)
    {}
}
