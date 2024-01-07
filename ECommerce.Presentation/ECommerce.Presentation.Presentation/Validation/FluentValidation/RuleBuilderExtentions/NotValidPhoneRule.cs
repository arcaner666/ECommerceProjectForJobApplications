using FluentValidation;
using System.Text.RegularExpressions;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;

public static class NotValidPhoneRule
{
    public static IRuleBuilderOptions<T, string> NotValidPhone<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        // Şuan kullanımda olan en kısa telefon numarası 7, en uzunu 15 karakterdir. 
        const string pattern = @"^[0-9]\d{6,14}$";
        var regex = new Regex(pattern);
        var options = ruleBuilder
            .Must(phone => regex.Match(phone).Success);
        return options;
    }
}