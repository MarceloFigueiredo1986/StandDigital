namespace StandDigital.Entities;

public class CarEntity
{
    public int CarroId { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public required string Combustivel { get; set; }
    public required string ImageMin { get; set; }
    public uint Kms { get; set; }
    public ushort Cvs { get; set; }
    public ushort? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public ushort? Cilindrada { get; set; }
    public required ushort Portas { get; set; }
    public required string Cor { get; set; }
    public uint Preco { get; set; }
    public ushort MesRegisto { get; set; }
    public ushort Ano { get; set; }
    public required ushort Lotacao { get; set; }
    public string? Caixa { get; set; }
    public required string Condicao { get; set; }
}
