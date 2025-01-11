using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.EM.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UserSalaService(AppDbContext appDbContext)
        : ICS_UserSala<DtoCreateUserSala, DtoGetUserSala>
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public string Create(DtoCreateUserSala dto)
        {
            var entity = dto.ToEntity();
            try
            {
                _appDbContext.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "Entidade gerada!";
        }

        public async Task<string> Update(DtoCreateUserSala dto, int userId, int salaId)
        {
            //A entidade existe, se nao solta um erro e nem passa dessa linha
            var entity = await CreateQuery()
                .Where(e => e.SalaId == userId)
                .Where(e => e.UserId == salaId)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Entidade nao encontrda!");

            entity.SalaId = dto.SalaId;
            entity.SalaId = dto.UserId;

            try
            {
                _appDbContext.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return entity.SalaId.ToString() + "-> User: " + entity.UserId;
        }

        public async Task Delete(int userId, int salaId)
        {
            var entity = CreateQuery()
                .Where(e => e.UserId == userId)
                .Where(e =>e.SalaId == salaId);
            try
            {
                _appDbContext.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<DtoGetUserSala>> GetByUserId(int id)
        {
            try
            {
                var dto = CreateQuery().Where(e => e.UserId == id).Select(e => e.ToDto()).AsNoTracking().ToList();
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DtoGetUserSala>> GetBySalaId(int id)
        {
            try
            {
                var dto = CreateQuery().Where(e => e.SalaId == id).Select(e => e.ToDto()).AsNoTracking().ToList();
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DtoGetUserSala>> GetList()
        {
            try
            {
                IQueryable<TblUsersSala> completeQuery = CreateQuery().AsQueryable();
                var listaDTO = await completeQuery.Select(c => c.ToDto()).ToListAsync();
                return listaDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.");
            }
        }



        //PRIVATE
        private IQueryable<TblUsersSala> CreateQuery()
        {
            return _appDbContext.TblUsersSala
            .AsNoTracking();
        }



        private async Task<TblUsersSala> GetEntityByUserIdAndThrowExIfNot(int userId)
        {
            return await CreateQuery()
                 .FirstOrDefaultAsync(e => e.UserId == userId) ?? throw new KeyNotFoundException("Entidade não encontrada");
        }

        private async Task<TblUsersSala> GetEntityBySalaIdAndThrowExIfNot(int salaId)
        {
            return await CreateQuery()
                 .FirstOrDefaultAsync(e => e.SalaId == salaId) ?? throw new KeyNotFoundException("Entidade não encontrada");
        }

    }
}


