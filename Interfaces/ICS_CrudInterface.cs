namespace WAPI_GS.Interfaces
{
    public interface ICS_CrudInterface<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        Task<string> CreateAsync(DtoCreateUpdate dto);
        Task<string> UpdateAsync(DtoCreateUpdate dto, int id);
        Task<DtoGet> GetByIdAsync(int id);
        Task<PagedList<DtoGet>> GetListAsync(FiltersParameter filtersParameter);
        Task DeleteAsync(int id);
    }
}
