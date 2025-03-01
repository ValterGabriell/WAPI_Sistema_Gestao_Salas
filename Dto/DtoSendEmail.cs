namespace WAPI_GS.Dto
{
    public class DtoSendEmail
    {
        public string destEmail { get; set; }
        public string body { get; set; }
        public string salaNome { get; set; }
        public int salaId { get; set; }
        public DateOnly dia { get; set; }
        public int currentUserId { get; set; }
        public string currentUsername { get; set; }
        public int horaInit { get; set; }
        public int horaFinal { get; set; }
    }
}
