namespace StandDigital.Core.Entities;

public class DestaquesEntity
{
    public int DestaquesId { get; set; }
    public int CarroId { get; set; }
    public DateTimeOffset DataInicioDestaque { get; set; }
    public DateTimeOffset DataFimDestaque { get; set; }
}
