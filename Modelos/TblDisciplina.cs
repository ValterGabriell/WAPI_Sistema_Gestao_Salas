using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos
{
    [Table("tbldisciplina")]
    public class TblDisciplina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("codigo")]
        public string Codigo { get; set; }
        [Column("sigla")]
        public string Sigla { get; set; }
        [Column("cargahoraria")]
        public int CargaHoraria { get; set; }
        [Column("totalaulas")]
        public int TotalAulas { get; set; }
    }
}
