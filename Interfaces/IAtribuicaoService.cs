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
    }
}
