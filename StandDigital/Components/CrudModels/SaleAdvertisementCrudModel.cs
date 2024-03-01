using System.ComponentModel.DataAnnotations;

namespace StandDigital.Components.CrudModels;

public class SaleAdvertisementCrudModel
{
    [Required(ErrorMessage = "Escolha a Marca")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "Escolha o Modelo")]
    public required string Modelo { get; set; }

    [Required(ErrorMessage = "Escolha o Combustível")]
    public required string Combustivel { get; set; }

    [Required(ErrorMessage = "Insira os quilómetros")]
    public int? Kms { get; set; }
    public short? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public short? Cilindrada { get; set; }
    public string? Caixa { get; set; }

    [Required(ErrorMessage = "Insira a potência em cavalos")]
    public short? Cv { get; set; }

    [Required(ErrorMessage = "Escolha o nº. de portas")]
    public short? Portas { get; set; }

    [Required(ErrorMessage = "Escolha o mês de registo")]
    public short? MesRegisto { get; set; }

    [Required(ErrorMessage = "Escolha a lotação")]
    public short? Lotacao { get; set; }

    [Required(ErrorMessage = "Escolha o ano da viatura")]
    public short? Ano { get; set; }

    [Required(ErrorMessage = "Insira a matrícula")]
    public string? Matricula { get; set; }

    public string? Comentarios { get; set; }

    [Required(ErrorMessage = "Escolha se é só teve 1 dono")]
    public short? PrimeiroDono { get; set; }

    public string? Danos { get; set; }
    public string? ProblemasMotor { get; set; }

    [Required(ErrorMessage = "Escolha se é nacional ou importado")]
    public short Nacional { get; set; } = 1;

}
