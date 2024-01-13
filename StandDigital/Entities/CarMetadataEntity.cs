namespace StandDigital.Entities;

public class CarMetadataEntity
{
    public int CarroMetadataId { get; set; }
    public int CarroId { get; set; }
    public string? DataMatricula { get; set; }
    public string? DataVenda { get; set; }
    public int Ativo { get; set; }
    public int CliquesAnuncio { get; set; }
}