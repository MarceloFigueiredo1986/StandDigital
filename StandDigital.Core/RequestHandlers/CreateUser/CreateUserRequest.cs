
using ErrorOr;
using MediatR;
using StandDigital.Core.DataValidators;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.RequestHandlers.User;

public class CreateUserRequest : IRequest<ErrorOr<bool>>
{
    [Required(ErrorMessage = "Escolha um nome")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Insira uma morada")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Insira um número de contribuinte")]
    [VatAttributeValidator(ErrorMessage = "Número contribuinte inválido")]
    public string? NIF { get; set; }
 
    [Required(ErrorMessage = "Preencha o email")]
    [EmailAddress(ErrorMessage = "Insira um email válido")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Preencha o número de telefone")]
    [MobileValidator(ErrorMessage = "O número de telefone é inválido")] 
    public long? Mobile { get; set; }

    [Required(ErrorMessage = "Preencha o nome de utilizador")]
    public string? Username { get; set; }
   
    [Required(ErrorMessage = "Preencha uma password")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Z])(?=.*\\d).{8,}$",
        ErrorMessage ="Deve conter 1 maiúscula, 1 número e pelo menos 8 caracteres")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Preencha a confirmação da password")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "As passwords não coincidem")]
    public string? ConfirmPassword { get; set; }
}
