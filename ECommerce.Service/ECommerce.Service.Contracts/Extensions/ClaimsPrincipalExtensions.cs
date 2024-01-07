using System.Security.Claims;

namespace ECommerce.Service.Contracts.Extensions;

public static class ClaimsPrincipalExtensions
{
    private static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    public static long PersonId(this ClaimsPrincipal claimsPrincipal)
    {
        return Convert.ToInt64(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier).FirstOrDefault());
    }

    public static string Role(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Name).FirstOrDefault();
    }

    public static List<string> RoleClaims(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Role);
    }
}