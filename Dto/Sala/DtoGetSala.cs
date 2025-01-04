using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.Sala
{
    public class DtoGetSala
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public string? Name { get; set; }

        public ICollection<TblUsersSala> UserSalas { get; set; }
    }
}
