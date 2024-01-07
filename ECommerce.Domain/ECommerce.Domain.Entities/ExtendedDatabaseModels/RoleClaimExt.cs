namespace ECommerce.Domain.Entities.ExtendedDatabaseModels;

public class RoleClaimExt
{
    public long RoleClaimId { get; set; }
    public short RoleId { get; set; }
    public int ClaimId { get; set; }

    // Extended With Role
    public string RoleTitle { get; set; }

    // Extended With Claim
    public string ClaimTitle { get; set; }
}