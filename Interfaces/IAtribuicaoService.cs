using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface IAtribuicaoService
    {
        Task<DtoResponseCreate> AtribuirProfessorASala(DtoAtribuirProfessorASala dto);
        Task<string> AtualizarAtribuicaoProfessorASala(DtoAtualizarAtribuicaoProfessorSala dto,
            int previousUserId, int SalaId);

        Task RemoverAtribuicaoProfessorSala(int userId, int salaId, string turmaID, DateOnly dateOnly);
        Task RemoverTodasAtribuicaoProfessorSala(int userId, int salaId, string turmaID);
        Task<List<DtoGetUserSala>> GetList();

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
        int horaFinal);

        Task<bool> Accept(
           int salaId, DateOnly dia, int antigoUsuarioID,
             int novoUsuarioID, int horaInit, int horaFinal);

        Task<bool> NotAccept(
          int salaId, int usuarioQueSolicitou);
    }
}
