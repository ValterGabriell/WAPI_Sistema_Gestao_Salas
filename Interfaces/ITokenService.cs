using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WAPI_GS.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateJWT(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
