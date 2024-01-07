using FluentValidation;
using System.Text.RegularExpressions;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;

public static class NotValidEmailRule
{
    public static IRuleBuilderOptions<T, string> NotValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        const string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        var regex = new Regex(pattern);
        var options = ruleBuilder
            .Must(email => regex.Match(email).Success);
        return options;
    }
}