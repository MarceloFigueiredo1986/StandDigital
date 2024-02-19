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
    public string DataFimAnuncio { get; set; }
    public int PrecisaPneus { get;set; }
    public string Comentarios { get;set; }
    public string Matricula { get;set; }
    public int Lotacao { get;set; }

    public string DiasParaFimAnuncio
    {
        get
        {
            if (DataFimAnuncio is null) return "";

            var timeLeft = DateTimeOffset.Parse(DataFimAnuncio) - DateTime.UtcNow;

            if (timeLeft.Days < 2) return $"{timeLeft.Hours}:{timeLeft.Minutes} horas";

            return $"{timeLeft.Days} dias";
        }
    }
}
