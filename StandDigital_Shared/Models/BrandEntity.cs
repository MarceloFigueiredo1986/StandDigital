namespace StandDigital_Shared.Models;

public record BrandEntity(int MarcaId, string Nome)
{
    public BrandEntity() : this(default,string.Empty)
    {

    }
}
