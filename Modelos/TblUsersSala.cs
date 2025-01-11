using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace WAPI_GS.Modelos;

[Table("tbluserssala")]
[PrimaryKey(nameof(UserId), nameof(SalaId))]
public class TblUsersSala
{
    [Column("userid")]
    public int UserId { get; set; }

    [Column("salaid")]
    public int SalaId { get; set; }
}
