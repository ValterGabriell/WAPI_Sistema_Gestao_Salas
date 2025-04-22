using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _appDbContext;

        public LoginService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> EsqueciSenha(string username, string novaSenha)
        {
            try
            {
                TblProfessor? tblProfessor = await _appDbContext.TblUsers
                    .Where(e => e.Username.Equals(username)).FirstOrDefaultAsync();

                if (tblProfessor is null) throw new KeyNotFoundException("Usuário não cadastrado!");

                tblProfessor.Password = BCrypt.Net.BCrypt.HashPassword(novaSenha);

                _appDbContext.Update(tblProfessor);
                await _appDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<DtoResponseToken> Login(DtoLoginModel model)
        {
            try
            {
                TblProfessor? tblProfessor = await _appDbContext.TblUsers
                    .Where(e => e.Username.Equals(model.UserName)).FirstOrDefaultAsync();

                if (tblProfessor is null) throw new KeyNotFoundException("Usuário não cadastrado!");


                string senhaHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                if (tblProfessor!.Password!.Equals(senhaHash) is false) throw new ArgumentException("Senha inválida");

                TblAuth? authEncontrada
                    = await _appDbContext.TblAuth.Where(e => e.Id.Equals(tblProfessor.Id.ToString())).FirstOrDefaultAsync();

                if (authEncontrada != null)
                {
                    _appDbContext.Remove(authEncontrada);
                    await _appDbContext.SaveChangesAsync();
                }

                TblAuth auth = new TblAuth(
                    id: tblProfessor.Id.ToString(),
                    isAdmin: tblProfessor.Username.Equals("admin"),
                    tokenAvailableUntil: DateTime.Now.AddHours(5).Millisecond);

                _appDbContext.TblAuth.Add(auth);
                await _appDbContext.SaveChangesAsync();

                return new DtoResponseToken
                {
                    IsAdmin = tblProfessor.Username.Equals("admin"),
                    Expiration = DateTime.Now.AddHours(5),
                    Token = new Guid().ToString(),
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Revoke(string username)
        {
            try
            {
                TblProfessor? tblProfessor = await _appDbContext.TblUsers
                    .Where(e => e.Username.Equals(username)).FirstOrDefaultAsync();

                if (tblProfessor is null) throw new KeyNotFoundException("Usuário não cadastrado!");

                TblAuth? authEncontrada
                    = await _appDbContext.TblAuth.Where(e => e.Id.Equals(tblProfessor.Id.ToString())).FirstOrDefaultAsync();

                if (authEncontrada != null)
                {
                    _appDbContext.Remove(authEncontrada);
                    await _appDbContext.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
