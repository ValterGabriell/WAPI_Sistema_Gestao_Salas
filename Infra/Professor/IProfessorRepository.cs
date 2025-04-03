using WAPI_GS.Dto.User;
using WAPI_GS.Modelos;

namespace WAPI_GS.Infra.Professor
{
    public interface IProfessorRepository
    {
        Task<string> CreateAsync(DtoCreateUpdateUser dto);
        Task<string> UpdateAsync(int id, DtoCreateUpdateUser dto);
        Task<string> ChangeActiveAsync(int id);
        Task DeleteAsync(int id);
        Task<TblProfessor> GetByIdAsync(int id);
        Task<(IEnumerable<TblProfessor>, int count)> GetListAsync(FiltersParameter filtersParameter);
    }
}
