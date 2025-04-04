using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Disciplina
{
    public class DisciplinaRepositoryImpl(AppDbContext appDbContext) : IDisciplinaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public string Create(TblDisciplina entity)
        {
            _appDbContext.Add(entity);
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        public string Update(TblDisciplina entity)
        {
            _appDbContext.Update(entity);
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        public async Task<List<TblDisciplina>> GetListAsync()
        {
            List<TblDisciplina> tblDisciplinas = await _appDbContext.TblDisciplina.ToListAsync();
            return tblDisciplinas;
        }
        public async Task<TblDisciplina> GetByIdAsync(int id)
        {
            return await RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(id);
        }
        public async Task<TblDisciplina?> RecuperaDisciplinaPorCodigo(string codigo)
        {
            return await _appDbContext.TblDisciplina
                .Where(x => x.Codigo == codigo)
                .FirstOrDefaultAsync();
        }


        public async Task<TblDisciplina> RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(int id)
        {
            return await _appDbContext.TblDisciplina
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException(HelperMessages.DISCIPLINA_NAO_ENCONTRADA);
        }


    }
}
