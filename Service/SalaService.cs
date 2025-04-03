using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.Sala;
using WAPI_GS.EM.Sala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class SalaService(AppDbContext appDbContext)
        : ICrudInterface<DtoCreateSala, DtoGetSala>
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<string> CreateAsync(DtoCreateSala dto)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
                var entity = dto.ToEntity();
                _appDbContext.Add(entity);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }

            return "Entidade gerada!";
        }

        public async Task<string> UpdateAsync(DtoCreateSala dto, int id)
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

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
            //A entidade existe, se nao solta um erro e nem passa dessa linha
            await GetEntityByIdAndThrowExIfNot(id);

            foreach (var item in CreateQueryByTenantAndActive().AsQueryable())
            {
                if (item.Name == dto.Name)
                {
                    throw new Exception("Sala com nome já cadastrado");
                }
            };

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


        public async Task<string> ChangeActive(int id)
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

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
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


        public async Task DeleteAsync(int id)
        {

            var entity = await _appDbContext.TblSalas.Where(e => e.Id == id).FirstAsync();
            try
            {

                _appDbContext.Remove(entity);

                List<TblPtd> tblUsersSalas = await _appDbContext.TblUsersSala.Where(e => e.SalaId == id).ToListAsync();
                _appDbContext.RemoveRange(tblUsersSalas);



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DtoGetSala> GetByIdAsync(int id)
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

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
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

        public async Task<PagedList<DtoGetSala>> GetListAsync(FiltersParameter filtersParameter)
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

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
            try
            {
                IQueryable<TblSala> completeQuery = CreateQueryByTenantAndActive().AsQueryable();

                completeQuery = FilteringWhenExistFilters(filtersParameter, completeQuery);

                var listaDTO = await completeQuery.Select(c => c.ToDto()).ToListAsync();

                var result = PagedList<DtoGetSala>.ToPagedList(
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
        private IQueryable<TblSala> CreateQueryByTenantAndActive()
        {
            return _appDbContext.TblSalas
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery();
        }

        private async Task<TblSala> GetEntityByIdAndThrowExIfNot(int id)
        {
            return await CreateQueryByTenantAndActive()
                 .FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Entidade não encontrada");
        }

        private IQueryable<TblSala> FilteringWhenExistFilters(FiltersParameter queryParameters, IQueryable<TblSala> query)
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
