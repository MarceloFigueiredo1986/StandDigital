namespace StandDigital_Shared.Entities;

public record BrandEntity(int MarcaId, string Nome)
{
    public BrandEntity() : this(default,string.Empty)
    {

    }
}
