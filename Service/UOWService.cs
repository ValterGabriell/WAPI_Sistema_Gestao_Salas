using CS950_ServiceCenter_WAPI.Interfaces;
using WAPI_GS.Dto.Sala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UOWService(AppDbContext appDbContext) : ICS_UnitOfWork
    {
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> _salaRepository;
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> SalaRepository 
            => _salaRepository ??= new SalaService(appDbContext);

        public async Task Commit()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
