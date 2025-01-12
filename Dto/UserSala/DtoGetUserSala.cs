namespace WAPI_GS.Dto.UserSala
{
    public class DtoGetUserSala
    {
        public int UserId { get; set; }
        public int SalaId { get; set; }
        public DateOnly Dia { get; set; }
    }
}
