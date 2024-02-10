namespace StandDigital.Models;

public class BaseCar
{
    public int CarroId { get; set; }
    public string Combustivel { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Kms { get; set; }
    public int MesRegisto { get; set; }
    public int Ano { get; set; }
    public string Danos { get; set; }
    public string ProblemasMotor { get; set; }
    public string ImageMin { get; set; }
    public ushort Nacional { get; set; }
    public ushort PrimeiroDono { get; set; }
    public string DataInicioAnuncio { get; set; }
    public int DiasParaFimAnuncio => DataInicioAnuncio is null ? 0 : (DateTimeOffset.Parse(DataInicioAnuncio).AddDays(14) - DateTimeOffset.Now).Days;
}
