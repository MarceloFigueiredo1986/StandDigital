using ErrorOr;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.RequestHandlers.LookingForDeals;

public class LookingForDealsQuery : IRequest<ErrorOr<bool>>
{
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Distrito { get; set; }
    public short Ano { get; set; }
    public int Kms { get; set; }
    public short Portas { get; set; }
    public short Lotacao { get; set; }
}