namespace StandDigital.Entities;

public class CarMetadataEntity
{
    public int CarroMetadataId { get; set; }
    public int CarroId { get; set; }
    public DateTime? DataInicioAnuncio { get; set; }
    public DateTime? DataFimAnuncio { get; set; }
    public DateTime? DataVenda { get; set; }
}