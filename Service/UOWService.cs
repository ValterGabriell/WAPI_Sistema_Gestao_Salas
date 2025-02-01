using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UOWService(AppDbContext appDbContext, IConfiguration configuration) : ICS_UnitOfWork
    {
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> _salaRepository = null!;
        public ICS_CrudInterface<DtoCreateSala, DtoGetSala> SalaRepository
            => _salaRepository ??= new SalaService(appDbContext);


        public ICS_CrudInterface<DtoCreateUser, DtoGetUser> _userRepository = null!;
        public ICS_CrudInterface<DtoCreateUser, DtoGetUser> UserRepository
            => _userRepository ??= new ProfessorService(appDbContext);


        public ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> _userSalaRepository = null!;
        public ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> UserSalaRepository
            => _userSalaRepository ??= new UserSalaService(appDbContext, configuration);

        public ICS_Auth AuthRepository => new AuthService(appDbContext);

        public ICS_Disciplina cS_Disciplina => new DisciplinaService(appDbContext);

        public async Task Commit()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
