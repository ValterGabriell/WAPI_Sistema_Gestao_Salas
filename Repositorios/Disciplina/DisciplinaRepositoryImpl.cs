using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Disciplina
{
    public class DisciplinaRepositoryImpl(AppDbContext appDbContext) : IDisciplinaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<string> CreateAsync(TblDisciplina entity)
        {
            TblDisciplina? tblDisciplina = await RecuperaDisciplinaPorCodigo(entity.Codigo);
            if (tblDisciplina != null) throw new InvalidDataException(HelperMessages.DISCIPLINA_JA_CADASTRADA);
            _appDbContext.Add(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        public async Task<string> UpdateAsync(TblDisciplina entity, int id)
        {
            TblDisciplina? tblDisciplina = await RecuperaDisciplinaPorCodigo(entity.Codigo);
            if (tblDisciplina != null) throw new InvalidDataException(HelperMessages.DISCIPLINA_JA_CADASTRADA);

            TblDisciplina? tblDisciplinaUpdate = await RecuperaDisciplinaPorID(id);
            if (tblDisciplinaUpdate != null) throw new KeyNotFoundException(HelperMessages.DISCIPLINA_NAO_ENCONTRADA);

            tblDisciplinaUpdate = entity;

            _appDbContext.Update(tblDisciplinaUpdate);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.DISCIPLINA_SALVO_SUCESSO;
        }

        public async Task<List<TblDisciplina>> GetListAsync()
        {
            List<TblDisciplina> tblDisciplinas = await _appDbContext.TblDisciplina.ToListAsync();
            return tblDisciplinas;
        }

        private async Task<TblDisciplina?> RecuperaDisciplinaPorCodigo(string codigo)
        {
            return await _appDbContext.TblDisciplina
                .Where(x => x.Codigo == codigo)
                .FirstOrDefaultAsync();
        }


        private async Task<TblDisciplina?> RecuperaDisciplinaPorID(int id)
        {
            return await _appDbContext.TblDisciplina
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

    }
}
