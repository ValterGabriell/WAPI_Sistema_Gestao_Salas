using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;

namespace WAPI_GS.Interfaces
{
    public interface ICS_CrudInterfaceController<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        ActionResult<string> Create(DtoCreateUpdate dto);
        Task<ActionResult<string>> Update(DtoCreateUpdate dto, int id);
        Task<ActionResult<DtoGet>> GetById(int id);
        Task<ActionResult<PagedList<DtoGet>>> GetList(FiltersParameter filtersParameter);
        Task<ActionResult> Delete(int id);
    }
}
