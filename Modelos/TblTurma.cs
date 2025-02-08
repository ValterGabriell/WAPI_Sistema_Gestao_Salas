using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos
{
    [Table("tblturma")]
    public class TblTurma
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = "";

        [Column("turno")]
        public string Turno { get; set; } = "";

        [Column("bloco")]
        public int Bloco { get; set; }

        [Column("nome")]
        public string Nome { get; set; }
    }
}
