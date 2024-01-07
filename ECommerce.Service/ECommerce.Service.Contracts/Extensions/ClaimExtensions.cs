using System.Security.Claims;

namespace ECommerce.Service.Contracts.Extensions;

public static class ClaimExtensions
{
    public static void AddPersonId(this ICollection<Claim> claims, string nameIdentifier)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
    }

    public static void AddRole(this ICollection<Claim> claims, string role)
    {
        claims.Add(new Claim(ClaimTypes.Name, role));
    }

    public static void AddRoleClaims(this ICollection<Claim> claims, IEnumerable<string> roleClaims)
    {
        roleClaims.ToList().ForEach(roleClaim => claims.Add(new Claim(ClaimTypes.Role, roleClaim)));
    }
}