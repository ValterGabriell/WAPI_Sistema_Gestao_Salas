using WAPI_GS.Dto.User;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Interfaces
{
    public interface IProfessorService
    {
        Task<string> CreateAsync(DtoCreateUpdateUser dto);
        Task<string> UpdateAsync(DtoCreateUpdateUser dto, int id);
        Task<string> ChangeActive(int id);
        Task DeleteAsync(int id);
        Task<PagedList<DtoGetProfessor>> GetListAsync(FiltersParameter filtersParameter);
        Task<DtoGetProfessor> GetByIdAsync(int id);
    }
}
