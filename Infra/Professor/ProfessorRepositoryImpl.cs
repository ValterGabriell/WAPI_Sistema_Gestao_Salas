using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.User;
using WAPI_GS.EM.User;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Infra.Professor
{
    public class ProfessorRepositoryImpl(AppDbContext appDbContext) : IProfessorRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<string> CreateAsync(DtoCreateUpdateUser dto)
        {
            try
            {
                TblProfessor professorEncontrado = await RecuperaProfessorPorEmailOuUsernameELancaExcecaoSeNaoExistir(dto);
                TblProfessor professorEntity = dto.ToEntity();

                professorEntity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                _appDbContext.Add(professorEntity);

                await _appDbContext.SaveChangesAsync();

                return HelperMessages.PROFESSOR_SALVO_SUCESSO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateAsync(int id, DtoCreateUpdateUser dto)
        {
            try
            {
                TblProfessor professorEncontrado = await RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                professorEncontrado = professorEncontrado.UpdateProfessorPropriedades(dto);

                _appDbContext.Update(professorEncontrado);

                await _appDbContext.SaveChangesAsync();

                return id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ChangeActiveAsync(int id)
        {
            try
            {
                TblProfessor? professorEncontrado = await RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                professorEncontrado = professorEncontrado.ChangeActivePropriedade();

                _appDbContext.Update(professorEncontrado);
                await _appDbContext.SaveChangesAsync();
                return id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                TblProfessor? professorEncontrado = await RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                _appDbContext.Remove(professorEncontrado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblProfessor> GetByIdAsync(int id)
        {
            try
            {
                TblProfessor? professorEncontrado = await RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                return professorEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(IEnumerable<TblProfessor>, int)> GetListAsync(FiltersParameter filtersParameter)
        {
            try
            {
                IQueryable<TblProfessor> query = from professor in _appDbContext.TblUsers
                                                 select professor;

                if (filtersParameter.IsActive is not null) query = query.Where(e => e.IsActive == filtersParameter.IsActive);
                if (filtersParameter.Search is not null) query = query.Where(e => e.Name == filtersParameter.Search);
                if (filtersParameter.Search is not null) query = query.Where(e => e.Email == filtersParameter.Search);

                query = query.PaginacaoNoBanco(filtersParameter.PageNumber, filtersParameter.PageSize);
                int count = query.GetCountTotal();

                query.AsNoTracking();

                return (await query.ToListAsync(), count);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }





        private async Task<TblProfessor> RecuperaProfessorPorEmailOuUsernameELancaExcecaoSeNaoExistir(DtoCreateUpdateUser dto)
        {
            TblProfessor? professorEncontrado = await (from professor in _appDbContext.TblUsers
                                                       where dto.Username == professor.Username || dto.Email == professor.Email
                                                       select professor).FirstOrDefaultAsync();
            if (professorEncontrado is null) throw new KeyNotFoundException(HelperMessages.PROFESSOR_NAO_ENCONTRADO);
            return professorEncontrado;
        }

        private async Task<TblProfessor> RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(int id)
        {
            TblProfessor? professorEncontrado = await (from professor in _appDbContext.TblUsers
                                                       where professor.Id == id
                                                       select professor).FirstOrDefaultAsync();
            if (professorEncontrado is null) throw new KeyNotFoundException(HelperMessages.PROFESSOR_NAO_ENCONTRADO);
            return professorEncontrado;

        }
    }
}
