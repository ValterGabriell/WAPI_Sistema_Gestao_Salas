using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UOWService(AppDbContext appDbContext) : ICS_UnitOfWork
    {
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> _salaRepository = null!;
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> SalaRepository 
            => _salaRepository ??= new SalaService(appDbContext);


        public ICS_CrudInterface<DtoCreateUser, DtoGetUser> _userRepository = null!;
        public ICS_CrudInterface<DtoCreateUser, DtoGetUser> UserRepository
            => _userRepository ??= new ProfessorService(appDbContext);


        public ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> _userSalaRepository = null!;
        public ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> UserSalaRepository 
            => _userSalaRepository ??= new UserSalaService(appDbContext);

        public async Task Commit()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
