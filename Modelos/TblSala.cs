
using System.ComponentModel.DataAnnotations.Schema;
using WAPI_GS.Dto.Sala;

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

    public TblSala MudaPropriedadeIsAtivo()
    {
        return new TblSala
        {
            Id = this.Id,
            IsActive = this.IsActive!,
            CreationDate = this.CreationDate,
            Name = this.Name,
        };
    }


    public DtoGetSala ToDto()
    {
        return new DtoGetSala
        {
            Id = this.Id,
            IsActive = this.IsActive!,
            CreationDate = this.CreationDate,
            Name = this.Name,
        };
    }
}
