using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Disciplina
{
    public interface IDisciplinaRepository
    {
        Task<string> CreateAsync(TblDisciplina entity);
        Task<string> UpdateAsync(TblDisciplina entity, int id);
        Task<List<TblDisciplina>> GetListAsync();
    }
}
