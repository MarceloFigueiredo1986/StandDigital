namespace StandDigital.Entities;

public record CarPhotoEntity(int CarroId, string Name, byte[] Data)
{
    public CarPhotoEntity() :this(default, default!, default!){}
}