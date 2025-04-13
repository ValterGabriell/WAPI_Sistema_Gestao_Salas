using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.ProfessorSala
{
    public class ProfessorSalaRepositoryImpl(AppDbContext appDbContext) : IProfessorSalaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;


        public async Task<string> AtribuirProfessorASala(TblPtd entity)
        {
            _appDbContext.Add(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.ATRIBUICAO_FEITA;
        }

        public async Task<string> AtualizarAtribuicaoProfessorSala(TblPtd entity)
        {
            _appDbContext.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.ATRIBUICAO_FEITA;
        }

        public async Task RemoverAtribuicaoProfessorSala(TblPtd entity)
        {
            _appDbContext.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TblPtd> RecuperarProfessorParaAtualizacaoSalaELancaExcecaoSeNaoEncontrar
            (DtoAtualizarAtribuicaoProfessorSala dto, int previousUserId, int SalaId)
        {
            TblPtd? tblProfessorSala = await (from _tblUserSala in _appDbContext.TblUsersSala
                                              where _tblUserSala.Dia == dto.DiaCorrente
                                                  && _tblUserSala.SalaId == SalaId
                                                  && _tblUserSala.UserId == previousUserId
                                              select _tblUserSala).FirstOrDefaultAsync();
            if (tblProfessorSala is null) throw new KeyNotFoundException(HelperMessages.ATRIBUICAO_NAO_ENCONTRADA);

            return tblProfessorSala;
        }

        public async Task<List<TblPtd>> RecuperarTodosProfessorSalaParaDiaParaDeletar(
            int userId, int salaId, string turmaId)
        {
            List<TblPtd> tblProfessorSalas = await (from _tblUserSala in _appDbContext.TblUsersSala
                                                    where _tblUserSala.SalaId == salaId
                                                        && _tblUserSala.UserId == userId
                                                        && _tblUserSala.TurmaId == turmaId
                                                    select _tblUserSala).ToListAsync();
            if (tblProfessorSalas.Count == 0) throw new KeyNotFoundException(HelperMessages.ATRIBUICAO_NAO_ENCONTRADA);

            return tblProfessorSalas;
        }

        public async Task<TblPtd> RecuperarProfessorSalaParaDiaParaDeletar(
          int userId, int salaId, string turmaId, DateOnly dia)
        {
            TblPtd? tblProfessorSalas = await (from _tblUserSala in _appDbContext.TblUsersSala
                                               where _tblUserSala.SalaId == salaId
                                                   && _tblUserSala.UserId == userId
                                                   && _tblUserSala.TurmaId == turmaId
                                                   && _tblUserSala.Dia == dia
                                               select _tblUserSala).FirstOrDefaultAsync();
            if (tblProfessorSalas is null) throw new KeyNotFoundException(HelperMessages.ATRIBUICAO_NAO_ENCONTRADA);

            return tblProfessorSalas;
        }

        public async Task<TblPtd> RecuperarProfessorSalaParaDeletarSalaELancaExcecaoSeNaoEncontrar(int userId, int salaId)
        {
            TblPtd? tblProfessorSala = await (from _tblUserSala in _appDbContext.TblUsersSala
                                              where _tblUserSala.SalaId == salaId
                                                  && _tblUserSala.UserId == userId
                                              select _tblUserSala).FirstOrDefaultAsync();
            if (tblProfessorSala is null) throw new KeyNotFoundException(HelperMessages.ATRIBUICAO_NAO_ENCONTRADA);

            return tblProfessorSala;
        }

        public async Task<List<TblPtd>> RecuperaTodasAsAtribuicoes()
        {
            List<TblPtd> tblAtribuicoesProfessorSala = await (from _tblProfessorSala in _appDbContext.TblUsersSala.AsNoTracking()
                                                              select _tblProfessorSala).ToListAsync();

            return tblAtribuicoesProfessorSala;
        }


        public bool VerificaSeEntidadeJaEstaAgendadaParaODia(DateOnly Dia, int HoraInicial, int HoraFinal)
        {
            return _appDbContext.TblUsersSala
                .AsNoTracking()
                .Any(e => e.Dia == Dia
                    && HoraInicial >= e.HoraInicial
                    && HoraFinal <= e.HoraFinal);
        }
    }
}


