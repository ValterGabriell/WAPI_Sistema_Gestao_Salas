using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UOWService(AppDbContext appDbContext, IConfiguration configuration) : ICS_UnitOfWork
    {
        public ICrudInterface<DtoCreateSala, DtoGetSala> _salaRepository = null!;
        public ICrudInterface<DtoCreateSala, DtoGetSala> SalaRepository
            => _salaRepository ??= new SalaService(appDbContext);


        public ICrudInterface<DtoCreateUpdateUser, DtoGetProfessor> _userRepository = null!;
        public ICrudInterface<DtoCreateUpdateUser, DtoGetProfessor> UserRepository
            => _userRepository ??= new ProfessorService(appDbContext);


        public ICS_UserSala<DtoAtribuirProfessorASala, DtoGetUserSala> _userSalaRepository = null!;
        public ICS_UserSala<DtoAtribuirProfessorASala, DtoGetUserSala> UserSalaRepository
            => _userSalaRepository ??= new UserSalaService(appDbContext, configuration);

        public ICS_Auth AuthRepository => new AuthService(appDbContext);

        public IDisciplinaService cS_Disciplina => new DisciplinaService(appDbContext);

        public async Task Commit()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
