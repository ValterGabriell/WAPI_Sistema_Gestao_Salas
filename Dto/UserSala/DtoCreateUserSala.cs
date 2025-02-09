namespace WAPI_GS.Dto.UserSala
{
    public class DtoCreateUserSala
    {
        public int UserId { get; set; }
        public int SalaId { get; set; }

        public int DisciplinaId { get; set; }
        public DateOnly Dia { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }
    }
}
