using Microsoft.EntityFrameworkCore;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class AuthService(AppDbContext appDbContext) : ICS_Auth
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<string> Login(string username, string password, bool isAdmin)
        {
            TblUser tblUser = await _appDbContext.TblUsers.Where(e => e.Username == username)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Usuário não encontrado");
            if (tblUser.Password != password)
            {
                throw new Exception("Senha incorreta");
            }


            string RequestToken = "";
            TblAuth? currentLogin = await _appDbContext.TblAuth.Where(e => e.UserId == tblUser.Id).FirstOrDefaultAsync() ?? null;

            if (currentLogin == null)
            {
                TblAuth tblAuth = await GenerateAuthEntityRegister(isAdmin, tblUser);

                RequestToken = tblAuth.RequestToken;
            }
            else
            {
                //token nao valido mais
                if (DateTime.UtcNow > currentLogin.TokenAvailableUntil)
                {
                    TblAuth tblAuth = await GenerateAuthEntityRegister(isAdmin, tblUser);
                    RequestToken = tblAuth.RequestToken;
                }
                else
                {
                    RequestToken = currentLogin.RequestToken;
                }
            }
            return RequestToken;
        }

        private async Task<TblAuth> GenerateAuthEntityRegister(bool isAdmin, TblUser tblUser)
        {
            tblUser.LastLogin = DateTime.Now;


            // Obter a data e hora atual
            DateTime agora = DateTime.Now;

            // Converter a data e hora atual para milissegundos (desde 1 de janeiro de 1970)
            long milissegundosAtuais = (long)(agora - new DateTime(1970, 1, 1)).TotalMilliseconds;

            // Adicionar 12 horas em milissegundos (12 horas * 60 minutos * 60 segundos * 1000 milissegundos)
            long milissegundosCom12Horas = milissegundosAtuais + (12 * 60 * 60 * 1000);




            TblAuth tblAuth = new()
            {
                Id = Guid.NewGuid().ToString(),
                IsAdmin = isAdmin,
                AccessToken = Guid.NewGuid().ToString(),
                TokenAvailableUntil = dateTime,
                UserId = tblUser.Id,
                RequestToken = Guid.NewGuid().ToString()
            };

            _appDbContext.Add(tblAuth);
            _appDbContext.Update(tblUser);
            await _appDbContext.SaveChangesAsync();
            return tblAuth;
        }
    }
}
