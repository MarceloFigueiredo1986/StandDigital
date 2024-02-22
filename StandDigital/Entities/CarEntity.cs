using System.Drawing;

namespace StandDigital.Entities;

public class CarEntity
{
    public int CarroId { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public required string Combustivel { get; set; }
    public int? FotoId { get; set; }
    public uint Kms { get; set; }
    public ushort Cv { get; set; }
    public ushort? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public ushort? Cilindrada { get; set; }
    public ushort Portas { get; set; }
    public ushort MesRegisto { get; set; }
    public ushort Lotacao { get; set; }
    public string? Caixa { get; set; }
    public ushort Ano { get; set; }
    public string Matricula { get; set; }
    public string? Comentarios { get; set; }
    public ushort PrimeiroDono { get; set; }
    public string? Danos { get; set; }
    public string? ProblemasMotor { get; set; }
    public ushort Nacional { get; set; }
    public decimal PrecoBase { get; set; }
    public int TipoNegocioId { get; set; }
    public byte[] Photo { get; set; }

    public Image ImageFromByteArray()
    {
        using var ms = new MemoryStream(Photo);
        ms.Position = 0;
        return Image.FromStream(ms);
    }
}