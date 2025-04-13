//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using WAPI_GS.Dto;
//using WAPI_GS.Interfaces;

//namespace WAPI_GS.Service
//{
//    public class LoginService(
//        ITokenService tokenService
//        ) : ILoginService
//    {
//        private const int EXPIRE_TIME_REFRESH_TOKEN = 8;
//        private readonly ITokenService _tokenService = tokenService;
//        public async Task<DtoResponseToken> Login(DtoLoginModel model)
//        {
//            try
//            {
//                //int tenantID = await _iSY001Repository.GetTenantByDominio(model.Dominio);

//                //var user = await _iSY001Repository.GetByUsernameAsync(model.UserName, tenantID);

//                //TEM Q VERIFICAR A SENHA AQUI UM DIA

//                if (user is not null)
//                {
//                    var userRoles = await _iSY001Repository.GetRolesAsync(user.Id, tenantID);

//                    var authClaims = new List<Claim>
//                {
//                new(ClaimTypes.Name, user.Sy001Nome!),
//                new(ClaimTypes.Email, user.Sy001Email!),
//                new(ClaimTypes.PrimarySid, user.Id!),
//                 new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//                    foreach (var userRole in userRoles)
//                    {
//                        authClaims.Add(new Claim(ClaimTypes.Role, userRole?.Label ?? ""));
//                    }

//                    //gera o token jwt
//                    var token = _tokenService.GenerateJWT(authClaims);

//                    //gera o refresh token
//                    var refreshToken = _tokenService.GenerateRefreshToken();


//                    //recupera o tempo de expiracao
//                    _ = int.TryParse(JwtVariableName.JWT_TOKEN_EXPIRE_TIME!,
//                                       out int refreshTokenValidityInMinutes);



//                    Csicp_Sy025? csicp_Sy025 = await _sY025Repository.GetSY025ByUserId(user.Id);
//                    if (csicp_Sy025 is null)
//                    {
//                        csicp_Sy025 = new Csicp_Sy025
//                        {
//                            Sy025Usuarioid = user.Id,
//                            Id = Guid.NewGuid().ToString(),
//                            Sy025Dtcreate = DateTime.UtcNow,
//                            Sy025Refreshexpiredtime = DateTime.UtcNow.AddHours(EXPIRE_TIME_REFRESH_TOKEN),
//                            Sy025Refreshtoken = refreshToken,
//                            TenantId = tenantID
//                        };
//                        await _sY025Repository.CreateAsync(csicp_Sy025);
//                        return new DtoResponseToken
//                        {
//                            Token = new JwtSecurityTokenHandler().WriteToken(token),
//                            Expiration = token.ValidTo
//                        };
//                    }

//                    //adiciona o refresh token
//                    csicp_Sy025.Sy025Refreshexpiredtime = DateTime.UtcNow.AddHours(EXPIRE_TIME_REFRESH_TOKEN);
//                    csicp_Sy025.Sy025Refreshtoken = refreshToken;

//                    await _sY025Repository.UpdateAsync(csicp_Sy025);
//                    return new DtoResponseToken
//                    {
//                        Token = new JwtSecurityTokenHandler().WriteToken(token),
//                        Expiration = token.ValidTo
//                    };

//                }
//                throw new Exception("Token nao foi gerado");
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<DtoResponseToken> RefreshToken(string username, int tenant)
//        {
//            try
//            {
//                if (username is null)
//                {
//                    throw new Exception("Invalid client request");
//                }

//                Csicp_Sy001? user = await _iSY001Repository.GetByUsernameAsync(username, tenant);

//                if (user is null) throw new KeyNotFoundException(Util.ENTITY_NOT_FOUND + " - Usuário: " + username);

//                var refreshTokenUser = await _sY025Repository.GetSY025ByUserId(user.Id);

//                if (refreshTokenUser is null) throw new KeyNotFoundException("Usuário não possui refresh token salvo");
//                if (refreshTokenUser.Sy025Refreshtoken is null) throw new AccessViolationException("Refresh token revogado!");
//                if (refreshTokenUser.Sy025Refreshexpiredtime < DateTime.UtcNow) throw new ApplicationException("Refresh Token Expirado, logue novamente!");


//                var userRoles = await _iSY001Repository.GetRolesAsync(user.Id, tenant);
//                IEnumerable<Csicp_Sy003Regra?> distinctUserRoles = userRoles.DistinctBy(e => e?.Label);
//                var authClaims = new List<Claim>
//                {
//                        new(ClaimTypes.Name, user.Sy001Nome!),
//                        new(ClaimTypes.Email, user.Sy001Email!),
//                        new(ClaimTypes.NameIdentifier, user.Id),
//                        new(ClaimTypes.Sid, user.TenantId.ToString()),
//                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//                foreach (var userRole in distinctUserRoles)
//                {
//                    authClaims.Add(new Claim(ClaimTypes.Role, userRole?.Label ?? ""));
//                }


//                var newAccessToken = _tokenService.GenerateJWT(authClaims);
//                var newRefreshToken = _tokenService.GenerateRefreshToken();

//                refreshTokenUser.Sy025Refreshtoken = newRefreshToken;
//                refreshTokenUser.Sy025Refreshexpiredtime = DateTime.UtcNow.AddHours(EXPIRE_TIME_REFRESH_TOKEN);

//                await _sY025Repository.UpdateAsync(refreshTokenUser);

//                return new DtoResponseToken
//                {
//                    Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
//                    Expiration = newAccessToken.ValidTo
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<bool> Revoke(string username, int tenant)
//        {
//            try
//            {
//                var user = await _iSY001Repository.GetByUsernameAsync(username, tenant) ?? throw new Exception("Invalid user name");
//                var tokenUser = await _sY025Repository.GetSY025ByUserId(user.Id);
//                if (tokenUser != null)
//                {
//                    tokenUser.Sy025Refreshtoken = null;
//                    tokenUser.Sy025Refreshexpiredtime = null;
//                    await _sY025Repository.UpdateAsync(tokenUser);
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(Util.CreateExceptionMessage(ex));
//            }
//        }
//    }
//}
