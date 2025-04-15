using WAPI_GS.Dto.Sala;

namespace WAPI_GS.Interfaces
{
    public interface ISalaService
    {
        Task<string> Create(DtoCreateSala dto);
        Task<string> UpdateAsync(DtoCreateSala dto, int id);
        Task<string> ChangeActive(int id);
        Task DeleteAsync(int id);
        Task<DtoGetSala> GetByIdAsync(int id);
        Task<IEnumerable<DtoGetSala>> GetListAsync(FiltersParameter filtersParameter);
    }
}
