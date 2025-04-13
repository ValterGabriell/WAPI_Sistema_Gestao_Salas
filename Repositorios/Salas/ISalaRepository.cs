using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Salas
{
    public interface ISalaRepository : ICommit
    {
        Task<string> Create(TblSala entity);
        Task<string> Update(TblSala entity);
        Task Delete(TblSala entity);
        Task<TblSala> GetByIdAsync(int id);
        Task<IEnumerable<TblSala>> GetListAsync(FiltersParameter filtersParameter);
        Task<TblSala> RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(int id);
    }
}
