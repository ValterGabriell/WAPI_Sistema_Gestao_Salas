using WAPI_GS.Dto.UserSala;
using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.ProfessorSala
{
    public interface IProfessorSalaRepository
    {
        Task<string> AtribuirProfessorASala(TblPtd entity);
        Task<string> AtualizarAtribuicaoProfessorSala(TblPtd entity);
        Task RemoverAtribuicaoProfessorSala(TblPtd entity);

        bool VerificaSeEntidadeJaEstaAgendadaParaODia(DateOnly Dia, int HoraInicial, int HoraFinal);
        Task<List<TblPtd>> RecuperaTodasAsAtribuicoes();
        Task<TblPtd> RecuperarProfessorSalaParaDiaParaDeletar(int userId, int salaId, string turmaId, DateOnly dia);
        Task<List<TblPtd>> RecuperarTodosProfessorSalaParaDiaParaDeletar(int userId, int salaId, string turmaId);
        Task<TblPtd> RecuperarProfessorParaAtualizacaoSalaELancaExcecaoSeNaoEncontrar(DtoAtualizarAtribuicaoProfessorSala dto, int previousUserId, int SalaId);
        Task<TblPtd> RecuperarProfessorSalaParaDeletarSalaELancaExcecaoSeNaoEncontrar(int userId, int salaId);
    }
}
