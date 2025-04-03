using WAPI_GS.Dto.User;
using WAPI_GS.Modelos;

namespace WAPI_GS.Infra.Professor
{
    public interface IProfessorRepository
    {
        string Create(TblProfessor entity);
        string Update(TblProfessor entity);
        string ChangeActive(TblProfessor entity);
        void Delete(TblProfessor entity);
        Task<TblProfessor> GetByIdAsync(int id);
        Task<(IEnumerable<TblProfessor>, int count)> GetListAsync(FiltersParameter filtersParameter);

        Task<TblProfessor> RecuperaProfessorPorEmailOuUsernameELancaExcecaoSeNaoExistir(DtoCreateUpdateUser dto);
        Task<TblProfessor> RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(int id);
    }
}
