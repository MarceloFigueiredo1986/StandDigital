namespace StandDigital.Entities;

public record ExtraEntity(int ExtraId, string Nome)
{
    public ExtraEntity() : this(default, default!)
    {}
}