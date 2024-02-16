namespace StandDigital.Entities;

public record CarExtraEntity(int CarroExtraId, int CarroId)
{
    public CarExtraEntity() : this(default, default)
    {
        
    }
}
