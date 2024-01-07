using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.DatabaseModels;

[Table("role_claim")]
public class RoleClaim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("role_claim_id")]
    public long RoleClaimId { get; set; }
    [Column("role_id")]
    public short RoleId { get; set; }
    [Column("claim_id")]
    public int ClaimId { get; set; }
}