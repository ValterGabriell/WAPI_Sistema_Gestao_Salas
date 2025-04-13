using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WAPI_GS.Modelos;

[Table("tblptd")]
public class TblPtd
{

    [Column("id")]
    [Key]
    public string Id { get; set; } = null!;
    [Column("userid")]
    public int UserId { get; set; }

    [Column("salaid")]
    public int SalaId { get; set; }

    [Column("disciplinaid")]
    public int DisciplinaId { get; set; }

    [Column("dia")]
    public DateOnly Dia { get; set; }

    [Column("horainit")]
    public int HoraInicial { get; set; }

    [Column("horafinal")]
    public int HoraFinal { get; set; }

    public string? TurmaId { get; set; }
}
