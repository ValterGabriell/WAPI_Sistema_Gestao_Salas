using WAPI_GS.Dto.User;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class ProfessorService(IProfessorRepository professorRepository) : ICrudInterface<DtoCreateUpdateUser, DtoGetProfessor>
    {
        private readonly IProfessorRepository _professorRepository = professorRepository;

        public async Task<string> CreateAsync(DtoCreateUpdateUser dto)
        {
            try
            {
                string message = await _professorRepository.CreateAsync(dto);
                return message;
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
                string _id = await _professorRepository.UpdateAsync(id, dto);
                return _id;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        public async Task<string> ChangeActive(int id)
        {
            try
            {
                string _id = await _professorRepository.ChangeActiveAsync(id);
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
                await _professorRepository.DeleteAsync(id);
#warning REMOVER SALAS DO PROFESSOR
                //List<TblPtd> tblUsersSalas = await _appDbContext.TblUsersSala.Where(e => e.UserId == id).ToListAsync();
                //_appDbContext.RemoveRange(tblUsersSalas);
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
    }
}
