using WAPI_GS.Dto.UserSala;
using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.ProfessorSala
{
    public interface IProfessorSalaRepository
    {
        string AtribuirProfessorASala(TblPtd entity);
        string AtualizarAtribuicaoProfessorSala(TblPtd entity);
        void RemoverAtribuicaoProfessorSala(TblPtd entity);

        bool VerificaSeEntidadeJaEstaAgendadaParaODia(DateOnly Dia, int HoraInicial, int HoraFinal);
        Task<List<TblPtd>> RecuperaTodasAsAtribuicoes();
        Task<TblPtd> RecuperarProfessorParaAtualizacaoSalaELancaExcecaoSeNaoEncontrar(DtoAtualizarAtribuicaoProfessorSala dto, int previousUserId, int SalaId);
        Task<TblPtd> RecuperarProfessorSalaParaDeletarSalaELancaExcecaoSeNaoEncontrar(int userId, int salaId);
    }
}
