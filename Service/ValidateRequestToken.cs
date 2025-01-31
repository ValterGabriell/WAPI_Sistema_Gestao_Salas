using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public static class ValidateRequestToken
    {
        public static async Task<bool> Validate(AppDbContext appDbContext, string requestToken)
        {
            TblAuth? currentLogin = (await appDbContext.TblAuth.AsNoTracking()
                .Where(e => e.RequestToken == requestToken).FirstAsync() ?? null) ?? throw new Exception("Por favor, logue novamente");

            // Obter a data e hora atual
            DateTime agora = DateTime.Now;

            // Converter a data e hora atual para milissegundos (desde 1 de janeiro de 1970)
            long milissegundosAtuais = (long)(agora - new DateTime(1970, 1, 1)).TotalMilliseconds;

            if (milissegundosAtuais > currentLogin.TokenAvailableUntil)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
