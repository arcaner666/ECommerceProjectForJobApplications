using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.DatabaseModels;

[Table("claim")]
public class Claim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("claim_id")]
    public int ClaimId { get; set; }
    [Column("title")]
    public string Title { get; set; }
}