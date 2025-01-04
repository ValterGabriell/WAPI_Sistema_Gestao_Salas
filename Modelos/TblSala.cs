
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos;

public class TblSala
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public string? Name { get; set; }

    public ICollection<TblUsersSala> UserSalas { get; set; }
}
