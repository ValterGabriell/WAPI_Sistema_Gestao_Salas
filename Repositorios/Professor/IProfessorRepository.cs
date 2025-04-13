using WAPI_GS.Dto.User;
using WAPI_GS.Modelos;

namespace WAPI_GS.Infra.Professor
{
    public interface IProfessorRepository
    {
        Task<string> Create(TblProfessor entity);
        Task<string> Update(TblProfessor entity);
        Task<string> ChangeActive(TblProfessor entity);
        Task Delete(TblProfessor entity);
        Task<TblProfessor> GetByIdAsync(int id);
        Task<(IEnumerable<TblProfessor>, int count)> GetListAsync(FiltersParameter filtersParameter);
        Task<TblProfessor?> RecuperaProfessorPorEmailOuUsername(DtoCreateUpdateUser dto);
        Task<TblProfessor> RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(int id);
    }
}
