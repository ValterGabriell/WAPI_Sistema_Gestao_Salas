using WAPI_GS.Utilidades;

namespace WAPI_GS.Interfaces
{
    public interface ICrudInterface<TDtoCreateUpdate, TDtoGet>
        where TDtoGet : class
        where TDtoCreateUpdate : class
    {
        Task<string> CreateAsync(TDtoCreateUpdate dto);
        Task<string> UpdateAsync(TDtoCreateUpdate dto, int id);
        Task<TDtoGet> GetByIdAsync(int id);
        Task<PagedList<TDtoGet>> GetListAsync(FiltersParameter filtersParameter);
        Task DeleteAsync(int id);
    }
}
