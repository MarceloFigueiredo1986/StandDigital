using CountryValidation;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.DataValidators;

public class VatAttributeValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var countryValidator = new CountryValidator();
        var valid = countryValidator.ValidateIndividualTaxCode(value?.ToString(), Country.PT).IsValid;
        return !valid ?
            countryValidator.ValidateVAT(value?.ToString(), Country.PT).IsValid :
            valid;
    }
}
