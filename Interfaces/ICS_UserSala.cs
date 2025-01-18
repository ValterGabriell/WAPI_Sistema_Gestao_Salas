namespace WAPI_GS.Interfaces
{
    public interface ICS_UserSala<DtoCreateUpdate, DtoGet>
        where DtoGet : class where DtoCreateUpdate : class
    {
        string Create(DtoCreateUpdate dto);
        Task<string> Update(DtoCreateUpdate dto, int userId, int salaId);
        Task<List<DtoGet>> GetByUserId(int id);
        Task<List<DtoGet>> GetBySalaNome(string salaNome);
        Task<List<DtoGet>> GetList(int? salaId, int? profId);
        Task Delete(int userId, int salaId);
    }
}
