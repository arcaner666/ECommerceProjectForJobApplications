using FluentValidation;
using System.Text.RegularExpressions;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;

public static class NotValidPasswordRule
{
    public static IRuleBuilderOptions<T, string> NotValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        // En az bir büyük karakter.
        // En az bir küçük karakter.
        // En az bir rakam.
        // En az bir özel karakter => (  -._!`'#%&,:;<>=@{}~$()*+/\?[]^|  )
        // 8 ila 16 karakter sınırı.
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-._!#%&,:;<>=@~`{|}/'\$\*\+\?\^]+).{8,16}$";
        var regex = new Regex(pattern);
        var options = ruleBuilder
            .Must(password => regex.Match(password).Success);
        return options;
    }
}