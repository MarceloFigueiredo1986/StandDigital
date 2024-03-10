using System.ComponentModel.DataAnnotations;

namespace StandDigital.Core.DataValidators;

public class MobileValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var numero = phoneNumberUtil.Parse(value!.ToString(), "PT");
        return phoneNumberUtil.IsValidNumberForRegion(numero,"PT");
    }
}
