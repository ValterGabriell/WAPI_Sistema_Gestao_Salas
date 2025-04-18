using WAPI_GS.Dto;
using WAPI_GS.Dto.User;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class ProfessorService(IProfessorRepository professorRepository) : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository = professorRepository;

        public async Task<string> CreateAsync(DtoCreateUpdateUser dto)
        {
            try
            {
                TblProfessor? professorEncontrado =
                    await _professorRepository.RecuperaProfessorPorEmailOuUsername(dto);
                if (professorEncontrado is not null) throw new ArgumentException(HelperMessages.PROFESSOR_JA_CADASTRADO);

                professorEncontrado = dto.ToEntity();

                professorEncontrado.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                await _professorRepository.Create(professorEncontrado);

                return HelperMessages.PROFESSOR_SALVO_SUCESSO;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<string> UpdateAsync(DtoCreateUpdateUser dto, int id)
        {
            try
            {
                TblProfessor professorEncontrado =
                    await _professorRepository.RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);

                professorEncontrado = professorEncontrado.UpdateProfessorPropriedades(dto);

                await _professorRepository.Update(professorEncontrado);

                return id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<string> ChangeActive(int id)
        {
            try
            {
                TblProfessor professorEncontrado =
                   await _professorRepository.RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                string _id = await _professorRepository.ChangeActive(professorEncontrado);
                return _id;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                TblProfessor professorEncontrado =
                  await _professorRepository.RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                await _professorRepository.Delete(professorEncontrado);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<DtoGetProfessor> GetByIdAsync(int id)
        {
            try
            {
                TblProfessor professorEncontrado = await _professorRepository.GetByIdAsync(id);
                return professorEncontrado.ToDto();
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<PagedList<DtoGetProfessor>> GetListAsync(FiltersParameter filtersParameter)
        {
            try
            {
                (IEnumerable<TblProfessor> professorsList, int count) =
                    await _professorRepository.GetListAsync(filtersParameter);

                IEnumerable<DtoGetProfessor> professorDtos = professorsList.Select(e => e.ToDto()).AsEnumerable();

                PagedList<DtoGetProfessor> pagedProfessorsList = PagedList<DtoGetProfessor>
                    .ToPagedList(professorDtos, count, filtersParameter.PageNumber, filtersParameter.PageSize);


                return pagedProfessorsList;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<List<DtoGetCombo>> GetListCombo(FiltersParameter filtersParameter)
        {
            try
            {
                var (lista, c) = await _professorRepository.GetListAsync(filtersParameter);
                return lista.Select(e => new DtoGetCombo(e.Id.ToString(), e.Name ?? "-")).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}
