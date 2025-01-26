using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UserSala<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        DtoResponseCreate Create(DtoCreateUpdate dto);
        Task<string> Update(DtoUpdateSalaUser dto, int oldUserId, int SalaId);
        //Task<List<DtoGet>> GetByUserId(int id);
        //Task<List<DtoGet>> GetBySalaNome(string salaNome);
        Task<List<DtoGet>> GetList(int? salaId, int? profId);
        Task Delete(int userId, int salaId);

        Task<bool> SendEmail(string destEmail, string body, string title);
        Task<bool> SendEmailSolicitacao(string destEmail, string body, string title, string fullUrl, int salaId,
            DateOnly dia,
            int currentUserId,
            int newUserId,
            int horaInit,
            int horaFinal);
        Task<bool> Accept(int salaId, DateOnly dia, int userId, int newUserId, int horaInit, int horaFinal);
        Task<bool> NotAccept(int salaId);
    }
}
