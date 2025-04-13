using WAPI_GS.Dto.Turma;
using WAPI_GS.Modelos;

namespace WAPI_GS.Interfaces
{
    public interface ITurmaService
    {
        Task<string> CreateAsync(DtoCreateTurma dto);
        Task<TblTurma> GetByIdAsync(string id);
        Task<List<TblTurma>> GetListAsync();
        Task<string> Update(DtoCreateTurma dto, string id);
        Task<string> Delete(string id);
    }
}
