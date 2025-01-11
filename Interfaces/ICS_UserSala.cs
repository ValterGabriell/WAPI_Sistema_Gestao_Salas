using CS800_Model_iCorp;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UserSala<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        string Create(DtoCreateUpdate dto);
        Task<string> Update(DtoCreateUpdate dto, int userId, int salaId);
        Task<List<DtoGet>> GetByUserId(int id);
        Task<List<DtoGet>> GetBySalaId(int id);
        Task<List<DtoGet>> GetList();
        Task Delete(int userId, int salaId);
    }
}
