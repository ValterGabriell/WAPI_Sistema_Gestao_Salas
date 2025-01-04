using WAPI_GS.Dto.Sala;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UnitOfWork
    {
        ICS_CrudInterface<DtoCreateSala, DtoGetSala> SalaRepository { get; }
        Task Commit();
    }
}
