using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.DatabaseModels;

[Table("role")]
public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("role_id")]
    public short RoleId { get; set; }
    [Column("title")]
    public string Title { get; set; }
    [Column("detail")]
    public string Detail { get; set; }
}