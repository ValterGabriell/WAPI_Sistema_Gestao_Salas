using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos;

public class TblUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Name { get; set; }

    public string? MobilePhone { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
    public ICollection<TblUsersSala> UserSalas { get; set; }
}
