using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAPI_GS.Modelos
{
    [Table("tblauth")]
    public class TblAuth
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = "";

        [Column("isAdmin")]
        public bool IsAdmin { get; set; } = false;

        [Column("tokenAvailableUntil")]
        public long TokenAvailableUntil { get; set; }

        [Column("requestToken")]
        public string RefreshToken { get; set; } = "";

    }
}
