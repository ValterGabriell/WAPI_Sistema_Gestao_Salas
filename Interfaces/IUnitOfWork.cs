using WAPI_GS.Dto.Sala;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;

namespace WAPI_GS.Interfaces
{
    public interface IUnitOfWork
    {
        ICrudInterface<DtoCreateSala, DtoGetSala> SalaRepository { get; }
        ICrudInterface<DtoCreateUpdateUser, DtoGetProfessor> UserRepository { get; }
        ICS_UserSala<DtoAtribuirProfessorASala, DtoGetUserSala> UserSalaRepository { get; }
        ICS_Auth AuthRepository { get; }
        IDisciplinaService cS_Disciplina { get; }
        Task Commit();
    }
}
