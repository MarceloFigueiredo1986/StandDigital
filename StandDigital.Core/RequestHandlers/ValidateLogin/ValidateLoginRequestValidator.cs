using FluentValidation;

namespace StandDigital.Core.RequestHandlers.ValidateLogin;

public class ValidateLoginRequestValidator : AbstractValidator<ValidateLoginRequest>
{
    public ValidateLoginRequestValidator()
    {
            RuleFor(a => a.username).NotEmpty();
            RuleFor(a => a.password).NotEmpty();
    }
}
