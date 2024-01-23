namespace StandDigital_Shared.Entities;

public record CarExtra(int CarroExtraId, int CarroId)
{
    public CarExtra() : this(default, default)
    {
        
    }
}
