using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace WAPI_GS.Modelos;

[Table("tbluserssaladias")]
[PrimaryKey(nameof(UserId), nameof(SalaId))]
public class TblUsersSala
{
    [Column("userid")]
    public int UserId { get; set; }

    [Column("salaid")]
    public int SalaId { get; set; }

    [Column("dia")]
    public DateOnly Dia { get; set; }

    public TblUser TblUser { get; set; }
    public TblSala TblSala { get; set; }
}
