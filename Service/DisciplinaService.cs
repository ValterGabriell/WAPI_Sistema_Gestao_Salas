using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class DisciplinaService(IDisciplinaRepository repository) : IDisciplinaService
    {
        private readonly IDisciplinaRepository _repository = repository;
        public async Task<string> Create(DtoCreateDisciplina dto)
        {
            try
            {
                TblDisciplina newDisciplina = dto.ToEntity();
                string message = await _repository.Create(newDisciplina);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<string> UpdateAsync(DtoCreateDisciplina dto, int id)
        {
            try
            {
                TblDisciplina? tblDisciplina = await _repository.RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(id);
                tblDisciplina = dto.ToEntityForUpdate(id);

                string updateResult = await _repository.Update(tblDisciplina);
                return updateResult;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<List<TblDisciplina>> GetList()
        {
            try
            {
                List<TblDisciplina> tblDisciplinas = await _repository.GetListAsync();

#warning AQUI DEVE SE CRIAR UM DTO PRA DISCIPLINA E ISNERIR A TURMA QUE ELA TA
                //foreach (var curent in tblDisciplinas)
                //{
                //    TblTurma? turma = await _appDbContext.TblTurma.Where(e => e.Id == curent.TurmaId).FirstAsync();
                //    curent.tblTurma = turma;

                //}
                return tblDisciplinas;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

    }
}
