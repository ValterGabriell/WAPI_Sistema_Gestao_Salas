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

            TblAuth tblAdmin = await _appDbContext.TblAuth.SingleAsync(e => e.IsAdmin);

            if (isAdmin)
            {
                if (tblAdmin.UserId != tblUser.Id)
                {
                    throw new Exception("Usuário não encontrado!");
                }
            }
            else
            {
                if (tblAdmin.UserId == tblUser.Id)
                {
                    throw new Exception("Usuário não encontrado!");
                }
            }



            // Verifica se a senha está correta
            if (!BCrypt.Net.BCrypt.Verify(password, tblUser.Password))
            {
                throw new Exception("Senha incorreta");
            }


            string RequestToken = "";
            TblAuth? currentLogin = await _appDbContext.TblAuth.Where(e => e.UserId == tblUser.Id).FirstOrDefaultAsync() ?? null;

            // Obter a data e hora atual
            DateTime agora = DateTime.Now;

            // Converter a data e hora atual para milissegundos (desde 1 de janeiro de 1970)
            long milissegundosAtuais = (long)(agora - new DateTime(1970, 1, 1)).TotalMilliseconds;

            if (currentLogin == null)
            {
                TblAuth tblAuth = await GenerateAuthEntityRegister(isAdmin, tblUser, milissegundosAtuais);

                RequestToken = tblAuth.RequestToken;
            }
            else
            {


                if (milissegundosAtuais > currentLogin.TokenAvailableUntil)
                {
                    _appDbContext.Remove(currentLogin);
                    await _appDbContext.SaveChangesAsync();

                    TblAuth tblAuth = await GenerateAuthEntityRegister(isAdmin, tblUser, milissegundosAtuais);
                    RequestToken = tblAuth.RequestToken;
                }
                else
                {
                    RequestToken = currentLogin.RequestToken;
                }
            }
            return RequestToken;
        }

        private async Task<TblAuth> GenerateAuthEntityRegister(bool isAdmin, TblUser tblUser, long miliAtuais)
        {
            tblUser.LastLogin = DateTime.UtcNow;



            // Adicionar 12 horas em milissegundos (12 horas * 60 minutos * 60 segundos * 1000 milissegundos)
            long milissegundosCom12Horas = miliAtuais + (12 * 60 * 60 * 1000);




            TblAuth tblAuth = new()
            {
                Id = Guid.NewGuid().ToString(),
                IsAdmin = isAdmin,
                AccessToken = Guid.NewGuid().ToString(),
                TokenAvailableUntil = milissegundosCom12Horas,
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
