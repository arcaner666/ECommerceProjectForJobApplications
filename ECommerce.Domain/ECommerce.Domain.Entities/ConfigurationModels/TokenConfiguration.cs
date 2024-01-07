namespace ECommerce.Domain.Entities.ConfigurationModels;

public class TokenConfiguration
{
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int AccessTokenExpiration { get; set; }
    public string SecurityKey { get; set; } = string.Empty;
}
