namespace StandDigital.Entities;

public record CarPhoto(int CarroId, string Name, byte[] Data)
{
    public CarPhoto() :this(default, default!, default!){}
}