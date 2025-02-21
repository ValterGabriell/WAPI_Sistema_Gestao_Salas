using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos;

[Table("tbluser")]
public class TblUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("isactive")]
    public bool IsActive { get; set; }
    [Column("creationdate")]
    public DateTime CreationDate { get; set; }
    [Column("lastlogin")]
    public DateTime? LastLogin { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    [Column("mobilephone")]
    public string? MobilePhone { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("username")]
    public string? Username { get; set; }
    [Column("password")]
    public string? Password { get; set; }

    [Column("color")]
    public string? Color { get; set; }
}
