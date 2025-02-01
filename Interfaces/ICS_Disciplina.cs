using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Modelos;

namespace WAPI_GS.Interfaces
{
    public interface ICS_Disciplina
    {
        Task<string> Create(DtoCreateDisciplina dto, string requestKey);
        Task<string> Update(DtoCreateDisciplina dto, int id, string requestKey);
        Task<List<TblDisciplina>> GetList(string requestKey);

    }
}
