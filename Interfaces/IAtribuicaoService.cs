using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface IAtribuicaoService
    {
        Task<DtoResponseCreate> AtribuirProfessorASala(DtoAtribuirProfessorASala dto);
        Task<string> AtualizarAtribuicaoProfessorASala(DtoAtualizarAtribuicaoProfessorSala dto,
            int previousUserId, int SalaId);

        Task Delete(int userId, int salaId);
        Task<List<DtoGetUserSala>> GetList();
    }
}
