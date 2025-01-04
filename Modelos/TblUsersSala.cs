using Microsoft.EntityFrameworkCore;


namespace WAPI_GS.Modelos;

[PrimaryKey(nameof(UserId), nameof(SalaId))]
public class TblUsersSala
{
    public int UserId { get; set; }

    public int SalaId { get; set; }
    public TblUser User { get; set; }
    public TblSala Sala { get; set; }
}
