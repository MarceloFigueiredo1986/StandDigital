namespace StandDigital.Entities;

public class CarMetadataEntity
{
    public int CarroMetadataId { get; set; }
    public int CarroId { get; set; }
    public DateTime? DataAnuncio { get; set; }
    public DateTime? DataVenda { get; set; }
    public bool Ativo { get; set; }
}