namespace WAPI_GS.Dto.UserSala
{
    public class DtoUpdateSalaUser
    {
        public int UserId { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }
        public required string DiaCorrente { get; set; }
    }
}
