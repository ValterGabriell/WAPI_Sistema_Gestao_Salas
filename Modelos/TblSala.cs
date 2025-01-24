
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos;

[Table("tblsala")]
public class TblSala
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("isactive")]
    public bool IsActive { get; set; }

    [Column("creationdate")]
    public DateTime CreationDate { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}
