using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Service
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateJWT(IEnumerable<Claim> claims)
        {
            //recebe a chave secreta
            var key = "MYSUPERPOWERKEY";

            //converte a chave para array de bytes
            var privateKey = Encoding.UTF8.GetBytes(key);



            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                     SecurityAlgorithms.HmacSha256Signature);


            //descreve as informacoes para gerar o token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //informacoes usuario
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),

                /*
                 Audience (aud): Especifica para quem o token foi gerado, ou seja, a aplicação ou serviço que deve consumir o token.
                 Garante que o token será consumido apenas pela aplicação ou serviço correto,
                 evitando que um token destinado a um serviço seja usado em outro, o que poderia representar um risco de segurança.
                A API que consome o token (Audience) valida que o token foi emitido pela fonte correta 
                e que está sendo usado no serviço certo.
                 */
                Audience = Environment.GetEnvironmentVariable("API_URL"),

                /**
                 * Issuer (iss): Indica quem emitiu o token, 
                 * geralmente o servidor de autenticação ou serviço responsável pela geração do token.
                 * Garante que o token foi gerado por uma fonte confiável e legítima, 
                 * impedindo que um token falso, emitido por um atacante, seja aceito.
                 * A API que gera o token (Issuer) emite o JWT.
                 */
                Issuer = Environment.GetEnvironmentVariable("API_URL"),

                //credenciais de assinatura para assinar o token
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);


            return token;
        }

        //token de atualizacao usado para obter um novo token de acesso, facilita a experiencia do usuario
        //o usuario nao precisa inserir as credenciais novamente
        public string GenerateRefreshToken()
        {
            //cria um array de bytes 129 bytes
            var secureRandomBytes = new byte[128];

            //gera numeros aleatorios
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            //preenche o secure random bytes com os bytes de random number generator
            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }
    }

}