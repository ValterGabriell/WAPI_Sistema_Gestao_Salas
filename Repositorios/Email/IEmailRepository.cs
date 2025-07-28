namespace WAPI_GS.Repositorios.Email
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailSolicitacao(
            string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int antigoUsuarioID,
            int novoUsuarioID,
        int horaInit,
        int horaFinal
        );

        Task<bool> Accept(int salaId, DateOnly dia, int antigoUsuarioID,
             int novoUsuarioID, int horaInit, int horaFinal);
        Task<bool> SendEmail(string destEmail, string body, string title);
        Task<bool> NotAccept(int salaId);
    }
}
