namespace StandDigital_Shared.Entities;

public record BrandEntity(int MarcaId, string Nome, string Image)
{
    public BrandEntity() : this(default,string.Empty, string.Empty)
    {

    }
}
