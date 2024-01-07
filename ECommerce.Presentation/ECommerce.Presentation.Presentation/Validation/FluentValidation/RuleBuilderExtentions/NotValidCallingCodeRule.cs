using FluentValidation;
using System.Text.RegularExpressions;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;

public static class NotValidCallingCodeRule
{
    public static IRuleBuilderOptions<T, string> NotValidCallingCode<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        // Şuan kullanımda olan en kısa telefon numarası 7, en uzunu 15 karakterdir. 
        const string pattern = @"^\+[0-9]\d{0,5}$";
        var regex = new Regex(pattern);
        var options = ruleBuilder
            .Must(callingCode => regex.Match(callingCode).Success);
        return options;
    }
}