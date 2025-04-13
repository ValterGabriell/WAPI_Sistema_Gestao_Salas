using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WAPI_GS.Dto.User;

namespace WAPI_GS.Modelos;

[Table("tbluser")]
public class TblProfessor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("isactive")]
    public bool IsActive { get; set; }

    [Column("creationdate")]
    public DateTime CreationDate { get; set; }

    [Column("lastlogin")]
    public DateTime? LastLogin { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("mobilephone")]
    public string? MobilePhone { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("username")]
    public string? Username { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("color")]
    public string? Color { get; set; }

    [Column("isAdmin")]
    public string? IsAdmin { get; set; }

    public TblProfessor UpdateProfessorPropriedades(DtoCreateUpdateUser dto)
    {
        return new TblProfessor
        {
            Id = this.Id,
            CreationDate = this.CreationDate,
            Password = this.Password,
            Color = this.Color,
            IsAdmin = this.IsAdmin,
            Name = dto.Name,
            MobilePhone = dto.MobilePhone,
            Email = dto.Email,
            Username = dto.Username,
            IsActive = dto.IsActive ?? true,
        };
    }

    public TblProfessor ChangeActivePropriedade()
    {
        return new TblProfessor
        {
            Id = this.Id,
            CreationDate = this.CreationDate,
            Password = this.Password,
            Color = this.Color,
            IsAdmin = this.IsAdmin,
            Name = this.Name,
            MobilePhone = this.MobilePhone,
            Email = this.Email,
            Username = this.Username,
            IsActive = this.IsActive!,
        };
    }

    public DtoGetProfessor ToDto()
    {
        return new DtoGetProfessor
        {
            Id = this.Id,
            IsActive = this.IsActive,
            CreationDate = this.CreationDate,
            LastLogin = this.LastLogin,
            Name = this.Name,
            MobilePhone = this.MobilePhone,
            Email = this.Email,
            Username = this.Username,
            Color = this.Color,
        };
    }
}
