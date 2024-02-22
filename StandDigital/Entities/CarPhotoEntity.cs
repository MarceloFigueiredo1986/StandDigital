namespace StandDigital.Entities;

public record CarPhotoEntity(int CarroId, string Name, byte[] Data,bool Primary)
{
    public CarPhotoEntity() :this(default, default!, default!, default){}
}