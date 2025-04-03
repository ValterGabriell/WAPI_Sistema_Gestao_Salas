using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Salas
{
    public interface ISalaRepository : ICommit
    {
        string Create(TblSala entity);
        string Update(TblSala entity);
        void Delete(TblSala entity);
        Task<TblSala> GetByIdAsync(int id);
        Task<IEnumerable<TblSala>> GetListAsync(FiltersParameter filtersParameter);
        Task<TblSala> RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(int id);
    }
}
