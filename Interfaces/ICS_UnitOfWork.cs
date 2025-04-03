using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface ICS_UnitOfWork
    {
        ICrudInterface<DtoCreateSala, DtoGetSala> SalaRepository { get; }
        ICrudInterface<DtoCreateUpdateUser, DtoGetProfessor> UserRepository { get; }
        ICS_UserSala<DtoCreateUserSala, DtoGetUserSala> UserSalaRepository { get; }
        ICS_Auth AuthRepository { get; }
        IDisciplinaService cS_Disciplina { get; }
        Task Commit();
    }
}
