using ErrorOr;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.CarAdvertisement;

public class CarAdvertisementCrudModel : IRequest<ErrorOr<bool>>
{
    [Required(ErrorMessage = "Escolha a Marca")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "Escolha o Modelo")]
    public string? Modelo { get; set; }

    [Required(ErrorMessage = "Escolha o Combustível")]
    public string? Combustivel { get; set; }

    [Required(ErrorMessage = "Insira os quilómetros")]
    [Range(minimum:0,maximum:9999999)]
    public int? Kms { get; set; }
    public short? Bateria { get; set; }
    public string? TipoCarregamento { get; set; }
    public short? Cilindrada { get; set; }
    public string? Caixa { get; set; }

    [Required(ErrorMessage = "Insira a potência em cavalos")]
    [Range(minimum:1,maximum:9999, ErrorMessage = "Entre 1 e 9999")]
    public short? Cv { get; set; }

    [Required(ErrorMessage = "Escolha o nº. de portas")]
    [Range(minimum:1,maximum:7)]
    public short? Portas { get; set; }

    [Required(ErrorMessage = "Escolha o mês de registo")]
    [Range(minimum:1,maximum:12)]
    public short? MesRegisto { get; set; }

    [Required(ErrorMessage = "Escolha a lotação")]
    [Range(minimum:1,maximum:70)]
    public short? Lotacao { get; set; }

    [Required(ErrorMessage = "Escolha o ano da viatura")]
    [Range(minimum: 1900, maximum: 2099)]
    public int? Ano { get; set; } = DateTime.Now.Year;

    [Required(ErrorMessage = "Insira a matrícula")]
    public string? Matricula { get; set; }

    public string? Comentarios { get; set; }

    [Required(ErrorMessage = "Escolha um valor")]
    public short? PrimeiroDono { get; set; }

    public string? Danos { get; set; }
    public string? ProblemasMotor { get; set; }

    [Required(ErrorMessage = "Escolha um valor")]
    public short? Nacional { get; set; }

    [Range(minimum: 1, maximum: 1000000)]
    public decimal? PrecoBase { get; set; }

    [Required(ErrorMessage = "Escolha o tipo de negócio que pretende")]
    public int? TipoNegocioId { get; set; }
}