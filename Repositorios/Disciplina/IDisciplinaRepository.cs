using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Disciplina
{
    public interface IDisciplinaRepository
    {
        string Create(TblDisciplina entity);
        string Update(TblDisciplina entity);
        Task<List<TblDisciplina>> GetListAsync();
        Task<TblDisciplina> GetByIdAsync(int id);
        Task<TblDisciplina> RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(int id);
        Task<TblDisciplina?> RecuperaDisciplinaPorCodigo(string codigo);
    }
}
