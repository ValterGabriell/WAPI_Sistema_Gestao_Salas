using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Modelos;

namespace WAPI_GS.Interfaces
{
    public interface IDisciplinaService
    {
        Task<string> CreateAsync(DtoCreateDisciplina dto);
        Task<string> Update(DtoCreateDisciplina dto, int id);
        Task<List<TblDisciplina>> GetList(string requestKey);

    }
}
