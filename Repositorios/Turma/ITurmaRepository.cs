using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Turma
{
    public interface ITurmaRepository
    {
        Task<string> CreateAsync(TblTurma tblTurma);
        Task<TblTurma> GetByIdAsync(string id);
        Task<TblTurma?> GetForCreate(EnumTurnoTurma enumTurnoTurma, int bloco, string nome);
        Task<List<TblTurma>> GetListAsync();
        Task<bool> UpdateAsync(TblTurma tblTurma);
        Task<bool> DeleteAsync(string id);
    }
}
