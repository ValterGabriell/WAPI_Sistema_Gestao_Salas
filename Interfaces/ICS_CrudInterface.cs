using CS800_Model_iCorp;

namespace WAPI_GS.Interfaces
{
    public interface ICS_CrudInterface<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        string Create(DtoCreateUpdate dto);
        Task<string> Update(DtoCreateUpdate dto, int id);
        Task<DtoGet> GetById(int id);
        Task<PagedList<DtoGet>> GetList(FiltersParameter filtersParameter);
        Task Delete(int id);
    }
}
