namespace StandDigital.Core.Entities;

public record CarExtraEntity(int CarroExtraId, int CarroId,string Extra)
{
    public CarExtraEntity() : this(default, default, default!)
    {
        
    }
}
