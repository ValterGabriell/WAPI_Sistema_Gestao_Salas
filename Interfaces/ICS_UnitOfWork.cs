using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UnitOfWork
    {
        ICS_CrudInterface<DtoCreateSala, DtoGetSala> SalaRepository { get; }
        ICS_CrudInterface<DtoCreateUpdateUser, DtoGetProfessor> UserRepository { get; }
        ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> UserSalaRepository { get; }
        ICS_Auth AuthRepository { get; }
        ICS_Disciplina cS_Disciplina { get; }
        Task Commit();
    }
}
