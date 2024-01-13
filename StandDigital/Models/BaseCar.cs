namespace StandDigital.Models;

public class BaseCar
{
    public int CarroId { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Kms { get; set; }
    public int MesRegisto { get; set; }
    public int Ano { get; set; }
    public int Cv { get; set; }
    public string Combustivel { get; set; }
    public decimal Preco { get; set; }
    public string ImageMin { get; set; }
}
