using StandDigital.Enums;

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
    public ushort Lotacao { get; set; }
    public string? Caixa { get; set; }
    public string Condicao { get; set; }
}
