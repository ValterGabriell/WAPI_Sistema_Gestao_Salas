using WAPI_GS.Dto;

namespace WAPI_GS.Interfaces
{
    public interface ILoginService
    {
        Task<DtoResponseToken> Login(DtoLoginModel model);

        Task<bool> Revoke(string username);
        Task<bool> EsqueciSenha(string username, string novaSenha);
    }
}
