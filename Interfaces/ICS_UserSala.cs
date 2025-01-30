using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UserSala<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        Task<DtoResponseCreate> Create(DtoCreateUpdate dto, string requestKey);
        Task<string> Update(DtoUpdateSalaUser dto, int oldUserId, int SalaId, string requestKey);
        //Task<List<DtoGet>> GetByUserId(int id);
        //Task<List<DtoGet>> GetBySalaNome(string salaNome);
        Task<List<DtoGet>> GetList(int? salaId, int? profId, string requestKey);
        Task Delete(int userId, int salaId, string requestKey);

        Task<bool> SendEmail(string destEmail, string body, string title);
        Task<bool> SendEmailSolicitacao(string destEmail, string body, string title, string fullUrl, int salaId,
            DateOnly dia,
            int currentUserId,
            int newUserId,
            int horaInit,
            int horaFinal, string requestKey);
        Task<bool> Accept(int salaId, DateOnly dia, int userId, int newUserId, int horaInit, int horaFinal);
        Task<bool> NotAccept(int salaId);
    }
}
