using WAPI_GS.Dto;

namespace WAPI_GS.Interfaces
{
    public interface ILoginService
    {
        Task<DtoResponseToken> Login(DtoLoginModel model);
        Task<DtoResponseToken> RefreshToken(string username, int tenant);

        Task<bool> Revoke(string username, int tenant);
    }
}
