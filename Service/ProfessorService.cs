using CS800_Model_iCorp;
using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.User;
using WAPI_GS.EM.User;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class ProfessorService(AppDbContext appDbContext)
        : ICS_CrudInterface<DtoCreateUser, DtoGetUser>
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<string> Create(DtoCreateUser dto, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            var entity = dto.ToEntity();

            // Criptografa a senha antes de salvar
            entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
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

        public async Task<string> Update(DtoCreateUser dto, int id, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }


            //A entidade existe, se nao solta um erro e nem passa dessa linha
            await GetEntityByIdAndThrowExIfNot(id);

            var entity = dto.ToEntity();

            entity.Id = id;
            try
            {
                _appDbContext.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id.ToString();
        }


        public async Task<string> ChangeActive(int id, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            var entity = await GetEntityByIdAndThrowExIfNot(id);
            entity.IsActive = !entity.IsActive;
            try
            {
                _appDbContext.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id.ToString();
        }


        public async Task Delete(int id, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            DtoGetUser dtoGetUser = await GetById(id, requestKey);
            try
            {
                _appDbContext.Remove(dtoGetUser.ToEntity());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DtoGetUser> GetById(int id, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            try
            {
                var entity = await GetEntityByIdAndThrowExIfNot(id);
                var dto = entity.ToDto();
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedList<DtoGetUser>> GetList(FiltersParameter filtersParameter, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            try
            {
                IQueryable<TblUser> completeQuery = CreateQueryByTenantAndActive().AsQueryable();

                completeQuery = FilteringWhenExistFilters(filtersParameter, completeQuery);

                var listaDTO = await completeQuery.Select(c => c.ToDto()).ToListAsync();

                var result = PagedList<DtoGetUser>.ToPagedList(
                    listaDTO,
                    filtersParameter.PageNumber,
                    filtersParameter.PageSize
                    );

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.");
            }
        }



        //PRIVATE
        private IQueryable<TblUser> CreateQueryByTenantAndActive()
        {
            return _appDbContext.TblUsers
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery();
        }

        private async Task<TblUser> GetEntityByIdAndThrowExIfNot(int id)
        {
            return await CreateQueryByTenantAndActive()
                 .FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Entidade não encontrada");
        }

        private IQueryable<TblUser> FilteringWhenExistFilters(FiltersParameter queryParameters, IQueryable<TblUser> query)
        {
            if (queryParameters.Search != null)
            {
                query = query
                    .Where(entity => (entity.Name ?? "").Contains(queryParameters.Search ?? ""));
            }

            if (queryParameters.IsActive.HasValue)
            {
                query = query
                   .Where(entity => entity.IsActive == queryParameters.IsActive);
            }

            return query;
        }
    }
}
