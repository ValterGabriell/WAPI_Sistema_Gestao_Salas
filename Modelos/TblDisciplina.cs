using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos
{
    [Table("tbldisciplina")]
    public class TblDisciplina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Sigla { get; set; }
        public int CargaHoraria { get; set; }
        public int TotalAulas { get; set; }
    }
}
