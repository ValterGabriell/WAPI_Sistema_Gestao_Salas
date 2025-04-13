using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Disciplina
{
    public class DisciplinaRepositoryImpl(AppDbContext appDbContext) : IDisciplinaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<string> Create(TblDisciplina entity)
        {
            _appDbContext.Add(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        public async Task<string> Update(TblDisciplina entity)
        {
            RemoveInstanciaComIDIgualDoContextoLocalDoEF(entity);
            _appDbContext.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        private void RemoveInstanciaComIDIgualDoContextoLocalDoEF(TblDisciplina entity)
        {
            var local = _appDbContext.Set<TblDisciplina>()
                .Local
                .FirstOrDefault(entry => entry.Id == entity.Id);

            if (local != null)
            {
                // Desanexa a instância local para evitar conflito de tracking
                _appDbContext.Entry(local).State = EntityState.Detached;
            }
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
