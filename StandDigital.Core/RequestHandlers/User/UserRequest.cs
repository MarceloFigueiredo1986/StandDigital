using ErrorOr;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.RequestHandlers.User;

public class UserRequest : IRequest<ErrorOr<bool>>
{
    [Required(ErrorMessage = "Escolha um nome")]
    public string? Name { get; set; }

}
