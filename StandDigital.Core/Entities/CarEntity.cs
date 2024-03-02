namespace StandDigital.Core.Entities;

public record CarEntity(string Marca, string Modelo, string Combustivel, int Kms, short Cv, short Portas, short MesRegisto, short Lotacao,
    short Ano, string Matricula, short PrimeiroDono, short Nacional, int TipoNegocioId)
{
    public CarEntity() : this(default, default, default, default, default, default, default, default, default, default, default
        , default, default)
    {
        
    }

    public int CarroId { get; set; }
    public int? FotoId { get; set; }
    public short? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public short? Cilindrada { get; set; }
    public string? Caixa { get; set; }
    public string? Comentarios { get; set; }
    public string? Danos { get; set; }
    public string? ProblemasMotor { get; set; }
    public decimal? PrecoBase { get; set; }
}