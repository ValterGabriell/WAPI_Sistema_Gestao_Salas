using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.UserSala
{
    public class DtoGetUserSala
    {
        public int UserId { get; set; }
        public int SalaId { get; set; }
        public DateOnly Dia { get; set; }

        public TblUser TblUser { get; set; }
        public TblSala TblSala { get; set; }
    }
}
