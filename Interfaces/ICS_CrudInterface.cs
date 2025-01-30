using CS800_Model_iCorp;

namespace WAPI_GS.Interfaces
{
    public interface ICS_CrudInterface<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        Task<string> Create(DtoCreateUpdate dto, string requestKey);
        Task<string> Update(DtoCreateUpdate dto, int id, string requestKey);
        Task<DtoGet> GetById(int id, string requestKey);
        Task<PagedList<DtoGet>> GetList(FiltersParameter filtersParameter, string requestKey);
        Task Delete(int id, string requestKey);
    }
}
