namespace StandDigital.Entities;

public record CarPhotoEntity(int CarroId, byte[] Data,bool Primary)
{
    public CarPhotoEntity() :this(default, default!, default){}
}