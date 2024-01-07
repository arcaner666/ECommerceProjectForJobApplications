using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.DatabaseModels;

[Table("person")]
public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("person_id")]
    public long PersonId { get; set; }
    [Column("role_id")]
    public short RoleId { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("calling_code")]
    public string CallingCode { get; set; }
    [Column("phone")]
    public string Phone { get; set; }
    [Column("password_hash")]
    public byte[] PasswordHash { get; set; }
    [Column("password_salt")]
    public byte[] PasswordSalt { get; set; }
    [Column("refresh_token")]
    public string? RefreshToken { get; set; }
    [Column("refresh_token_expiry_time")]
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}