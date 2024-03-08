using CountryValidation;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.DataValidators;

public class VatAttributeValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var countryValidator = new CountryValidator();
        return countryValidator.ValidateVAT(value?.ToString(), Country.PT).IsValid;
    }
}
