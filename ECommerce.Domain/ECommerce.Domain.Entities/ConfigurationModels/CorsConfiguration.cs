namespace ECommerce.Domain.Entities.ConfigurationModels;

public class CorsConfiguration
{
    public string Mode { get; set; }
    public List<string> PermittedDomains { get; set; }
}
