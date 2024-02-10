namespace StandDigital.Entities;

public class CarEntity
{
    public int CarroId { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Combustivel { get; set; }
    public string ImageMin { get; set; }
    public uint Kms { get; set; }
    public ushort Cv { get; set; }
    public ushort? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public ushort? Cilindrada { get; set; }
    public ushort Portas { get; set; }
    public string Cor { get; set; }
    public uint Preco { get; set; }
    public ushort MesRegisto { get; set; }
    public ushort Ano { get; set; }
    public string Matricula { get; set; }
    public ushort Lotacao { get; set; }
    public string? Caixa { get; set; }
    public int Condicao { get; set; }
    public string? Comentarios { get; set; }
    public ushort PrecisaPneus { get; set; }
    public ushort PrimeiroDono { get; set; }
    public string? Danos { get; set; }
    public string? ProblemasMotor { get; set; }
    public ushort TemRevisao { get; set; }
    public ushort Nacional { get; set; }
}