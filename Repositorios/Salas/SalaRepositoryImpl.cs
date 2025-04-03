using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Salas
{
    public class SalaRepositoryImpl(AppDbContext appDbContext) : ISalaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public string Create(TblSala entity)
        {
            _appDbContext.Add(entity);
            return HelperMessages.SALA_SALVO_SUCESSO;
        }

        public string Update(TblSala entity)
        {
            _appDbContext.Update(entity);
            return HelperMessages.SALA_SALVO_SUCESSO;
        }
        public void Delete(TblSala entity)
        {
            _appDbContext.Remove(entity);
        }

        public async Task<TblSala> GetByIdAsync(int id)
        {
            var entity = await RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(id);
            return entity;
        }

        public async Task<IEnumerable<TblSala>> GetListAsync(FiltersParameter filtersParameter)
        {

            IQueryable<TblSala> completeQuery = CreateQueryByTenantAndActive();

            completeQuery = FilteringWhenExistFilters(filtersParameter, completeQuery);

            return await completeQuery.ToListAsync();
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }


        private IQueryable<TblSala> CreateQueryByTenantAndActive()
        {
            return _appDbContext.TblSalas
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task<TblSala> RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(int id)
        {
            return await CreateQueryByTenantAndActive()
                 .FirstOrDefaultAsync(e => e.Id == id) ??
                 throw new KeyNotFoundException(HelperMessages.SALA_NAO_ENCONTRADA);
        }

        private static IQueryable<TblSala> FilteringWhenExistFilters(FiltersParameter queryParameters,
            IQueryable<TblSala> query)
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

